﻿namespace Spect.Net.Assembler.SyntaxTree.Expressions
{
    /// <summary>
    /// This class represents an UNARY bitwise NOT operation
    /// </summary>
    public sealed class UnaryBitwiseNotNode : UnaryExpressionNode
    {
        /// <summary>
        /// Retrieves the value of the expression
        /// </summary>
        /// <param name="evalContext">Evaluation context</param>
        /// <returns>Evaluated expression value</returns>
        public override ushort Evaluate(IEvaluationContext evalContext) 
            => (ushort)~Operand.Evaluate(evalContext);
    }
}