﻿using System;
using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;
using Spect.Net.Assembler.Generated;
using Spect.Net.Assembler.SyntaxTree;
using Z80AsmParser = Spect.Net.Assembler.Generated.Z80AsmParser;

// ReSharper disable UsePatternMatching

// ReSharper disable JoinNullCheckWithUsage

namespace Spect.Net.Assembler.Assembler
{
    /// <summary>
    /// This class implements the Z80 assembler
    /// </summary>
    public partial class Z80Assembler
    {
        private string _sourceText;
        private AssemblerOptions _options;
        private AssemblerOutput _output;

        private CompilationUnit _parsedLines;
        private Stack<bool?> _ifdefStack;
        private bool _processOps;

        /// <summary>
        /// The condition symbols
        /// </summary>
        public HashSet<string> ConditionSymbols { get; private set; } = new HashSet<string>();

        /// <summary>
        /// Lines after running the preprocessor
        /// </summary>
        public List<SourceLineBase> PreprocessedLines { get; private set; }

        /// <summary>
        /// This method compiles the Z80 Assembly code in the specified file into Z80
        /// binary code.
        /// </summary>
        /// <param name="filename">Z80 assembly source file</param>
        /// <param name="options">
        /// Compilation options. If null is passed, the compiler uses the
        /// default options
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>
        /// Output of the compilation
        /// </returns>
        public AssemblerOutput CompileFile(string filename, AssemblerOptions options = null)
        {
            var sourceText = File.ReadAllText(filename);
            return Compile(sourceText, options);
        }


        /// <summary>
        /// This method compiles the passed Z80 Assembly code into Z80
        /// binary code.
        /// </summary>
        /// <param name="sourceText">Source code text</param>
        /// <param name="options">
        /// Compilation options. If null is passed, the compiler uses the
        /// default options
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>
        /// Output of the compilation
        /// </returns>
        public AssemblerOutput Compile(string sourceText, AssemblerOptions options = null)
        {

            // --- Init the compilation process
            _sourceText = sourceText ?? throw new ArgumentNullException(nameof(sourceText));
            _options = options ?? new AssemblerOptions();
            ConditionSymbols = new HashSet<string>(_options.PredefinedSymbols);
            _output = new AssemblerOutput();
            _output.StartCompilation();

            // --- Do the compilation phases
            if (!ProcessInclude() 
                || !ExecuteParse() 
                || !ExecuteDirectives() 
                || !EmitCode() 
                || !FixupSymbols())
            {
                // --- Compilation failed, remove segments
                _output.Segments.Clear();
            }
            _output.CompleteCompilation();
            return _output;
        }

        #region Include pragma

        /// <summary>
        /// Processes the #include directives, recursively
        /// </summary>
        /// <returns>
        /// True, if compilation may go on
        /// </returns>
        private bool ProcessInclude()
        {
            // TODO: Implement this method
            return true;
        }

        #endregion

        #region Parsing and Directive processing

        /// <summary>
        /// Parses the source code passed to the compiler
        /// </summary>
        /// <returns>True, if parsing was successful</returns>
        private bool ExecuteParse()
        {
            var inputStream = new AntlrInputStream(_sourceText);
            var lexer = new Z80AsmLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new Z80AsmParser(tokenStream);
            var context = parser.compileUnit();
            var visitor = new Z80AsmVisitor();
            visitor.Visit(context);
            _parsedLines = visitor.Compilation;
            foreach (var error in parser.SyntaxErrors)
            {
                ReportError(error);
            }
            return _output.ErrorCount == 0;
        }

        /// <summary>
        /// This method processes the parsed lines and creates a list of
        /// lines that should be used for code emitting according to
        /// the preprocessor directives
        /// </summary>
        /// <returns></returns>
        private bool ExecuteDirectives()
        {
            // --- Init the preprocessor
            var currentLineIndex = 0;
            PreprocessedLines = new List<SourceLineBase>();
            _ifdefStack = new Stack<bool?>();
            _processOps = true;

            // --- Traverse through parsed lines
            while (currentLineIndex < _parsedLines.Lines.Count)
            {
                var line = _parsedLines.Lines[currentLineIndex];
                var preProc = line as Directive;
                if (preProc != null)
                {
                    ApplyDirective(preProc);
                }
                else if (_processOps)
                {
                    PreprocessedLines.Add(line);
                }
                currentLineIndex++;
            }
            return _output.ErrorCount == 0;
        }

        /// <summary>
        /// Apply the specified prprocessor directive, and modify the
        /// current line index accordingly
        /// </summary>
        /// <param name="directive">Preprocessor directive</param>
        private void ApplyDirective(Directive directive)
        {
            if (directive.Mnemonic == "#DEFINE" && _processOps)
            {
                // --- Define a symbol
                ConditionSymbols.Add(directive.Identifier);
            }
            else if (directive.Mnemonic == "#UNDEF" && _processOps)
            {
                // --- Remove a symbol
                if (_processOps) ConditionSymbols.Remove(directive.Identifier);
            }
            else if (directive.Mnemonic == "#IFDEF" || directive.Mnemonic == "#IFNDEF" 
                || directive.Mnemonic == "#IF")
            {
                // --- Evaluate the condition and stop/start processing
                // --- operations accordingly
                if (_processOps)
                {
                    if (directive.Mnemonic == "#IF")
                    {
                        var value = EvalImmediate(directive, directive.Expr);
                        _processOps = value != null && value.Value != 0;
                    }
                    else
                    {
                        _processOps = ConditionSymbols.Contains(directive.Identifier) ^
                                      directive.Mnemonic == "#IFNDEF";
                    }
                    _ifdefStack.Push(_processOps);
                }
                else
                {
                    // --- Do not process after #else or #endif
                    _ifdefStack.Push(null);
                }
            }
            else if (directive.Mnemonic == "#ELSE")
            {
                if (_ifdefStack.Count == 0)
                {
                    ReportError(Errors.Z0060, directive);
                }
                else
                {
                    // --- Process operations according to the last
                    // --- condition's value
                    var peekVal = _ifdefStack.Pop();
                    if (peekVal.HasValue)
                    {
                        _processOps = !peekVal.Value;
                        _ifdefStack.Push(_processOps);
                    }
                    else
                    {
                        _ifdefStack.Push(null);
                    }
                }
            }
            else if (directive.Mnemonic == "#ENDIF")
            {
                if (_ifdefStack.Count == 0)
                {
                    ReportError(Errors.Z0061, directive);
                }
                else
                {
                    // --- It is the end of an #ifden/#ifndef block
                    _ifdefStack.Pop();
                    // ReSharper disable once PossibleInvalidOperationException
                    _processOps = _ifdefStack.Count == 0 || _ifdefStack.Peek().HasValue && _ifdefStack.Peek().Value;
                }
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Translates a Z80AsmParserErrorInfo instance into an error
        /// </summary>
        private void ReportError(Z80AsmParserErrorInfo error)
        {
            _output.Errors.Add(new AssemblerErrorInfo(error));
        }

        /// <summary>
        /// Reports the specified error
        /// </summary>
        /// <param name="errorCode">Code of error</param>
        /// <param name="line">Source line associated with the error</param>
        /// <param name="parameters">Optiona error message parameters</param>
        private void ReportError(string errorCode, SourceLineBase line, params object[] parameters)
        {
            _output.Errors.Add(new AssemblerErrorInfo(errorCode, line, parameters));
        }

        #endregion
    }
}