﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AntlrZ80Asm.Test.Assembler
{
    [TestClass]
    public class AluOperationEmitTests : AssemblerTestBed
    {
        [TestMethod]
        public void AluOpsWithRegsWorkAsExpected()
        {
            CodeEmitWorks("add a,b", 0x80);
            CodeEmitWorks("add a,c", 0x81);
            CodeEmitWorks("add a,d", 0x82);
            CodeEmitWorks("add a,e", 0x83);
            CodeEmitWorks("add a,h", 0x84);
            CodeEmitWorks("add a,l", 0x85);
            CodeEmitWorks("add a,(hl)", 0x86);
            CodeEmitWorks("add a,a", 0x87);
            CodeEmitWorks("add a,xh", 0xDD, 0x84);
            CodeEmitWorks("add a,xl", 0xDD, 0x85);
            CodeEmitWorks("add a,yh", 0xFD, 0x84);
            CodeEmitWorks("add a,yl", 0xFD, 0x85);

            CodeEmitWorks("adc a,b", 0x88);
            CodeEmitWorks("adc a,c", 0x89);
            CodeEmitWorks("adc a,d", 0x8A);
            CodeEmitWorks("adc a,e", 0x8B);
            CodeEmitWorks("adc a,h", 0x8C);
            CodeEmitWorks("adc a,l", 0x8D);
            CodeEmitWorks("adc a,(hl)", 0x8E);
            CodeEmitWorks("adc a,a", 0x8F);
            CodeEmitWorks("adc a,xh", 0xDD, 0x8C);
            CodeEmitWorks("adc a,xl", 0xDD, 0x8D);
            CodeEmitWorks("adc a,yh", 0xFD, 0x8C);
            CodeEmitWorks("adc a,yl", 0xFD, 0x8D);

            CodeEmitWorks("sbc a,b", 0x98);
            CodeEmitWorks("sbc a,c", 0x99);
            CodeEmitWorks("sbc a,d", 0x9A);
            CodeEmitWorks("sbc a,e", 0x9B);
            CodeEmitWorks("sbc a,h", 0x9C);
            CodeEmitWorks("sbc a,l", 0x9D);
            CodeEmitWorks("sbc a,(hl)", 0x9E);
            CodeEmitWorks("sbc a,a", 0x9F);
            CodeEmitWorks("sbc a,xh", 0xDD, 0x9C);
            CodeEmitWorks("sbc a,xl", 0xDD, 0x9D);
            CodeEmitWorks("sbc a,yh", 0xFD, 0x9C);
            CodeEmitWorks("sbc a,yl", 0xFD, 0x9D);

            CodeEmitWorks("sub b", 0x90);
            CodeEmitWorks("sub c", 0x91);
            CodeEmitWorks("sub d", 0x92);
            CodeEmitWorks("sub e", 0x93);
            CodeEmitWorks("sub h", 0x94);
            CodeEmitWorks("sub l", 0x95);
            CodeEmitWorks("sub (hl)", 0x96);
            CodeEmitWorks("sub a", 0x97);
            CodeEmitWorks("sub xh", 0xDD, 0x94);
            CodeEmitWorks("sub xl", 0xDD, 0x95);
            CodeEmitWorks("sub yh", 0xFD, 0x94);
            CodeEmitWorks("sub yl", 0xFD, 0x95);

            CodeEmitWorks("and b", 0xA0);
            CodeEmitWorks("and c", 0xA1);
            CodeEmitWorks("and d", 0xA2);
            CodeEmitWorks("and e", 0xA3);
            CodeEmitWorks("and h", 0xA4);
            CodeEmitWorks("and l", 0xA5);
            CodeEmitWorks("and (hl)", 0xA6);
            CodeEmitWorks("and a", 0xA7);
            CodeEmitWorks("and xh", 0xDD, 0xA4);
            CodeEmitWorks("and xl", 0xDD, 0xA5);
            CodeEmitWorks("and yh", 0xFD, 0xA4);
            CodeEmitWorks("and yl", 0xFD, 0xA5);

            CodeEmitWorks("xor b", 0xA8);
            CodeEmitWorks("xor c", 0xA9);
            CodeEmitWorks("xor d", 0xAA);
            CodeEmitWorks("xor e", 0xAB);
            CodeEmitWorks("xor h", 0xAC);
            CodeEmitWorks("xor l", 0xAD);
            CodeEmitWorks("xor (hl)", 0xAE);
            CodeEmitWorks("xor a", 0xAF);
            CodeEmitWorks("xor xh", 0xDD, 0xAC);
            CodeEmitWorks("xor xl", 0xDD, 0xAD);
            CodeEmitWorks("xor yh", 0xFD, 0xAC);
            CodeEmitWorks("xor yl", 0xFD, 0xAD);

            CodeEmitWorks("or b", 0xB0);
            CodeEmitWorks("or c", 0xB1);
            CodeEmitWorks("or d", 0xB2);
            CodeEmitWorks("or e", 0xB3);
            CodeEmitWorks("or h", 0xB4);
            CodeEmitWorks("or l", 0xB5);
            CodeEmitWorks("or (hl)", 0xB6);
            CodeEmitWorks("or a", 0xB7);
            CodeEmitWorks("or xh", 0xDD, 0xB4);
            CodeEmitWorks("or xl", 0xDD, 0xB5);
            CodeEmitWorks("or yh", 0xFD, 0xB4);
            CodeEmitWorks("or yl", 0xFD, 0xB5);

            CodeEmitWorks("cp b", 0xB8);
            CodeEmitWorks("cp c", 0xB9);
            CodeEmitWorks("cp d", 0xBA);
            CodeEmitWorks("cp e", 0xBB);
            CodeEmitWorks("cp h", 0xBC);
            CodeEmitWorks("cp l", 0xBD);
            CodeEmitWorks("cp (hl)", 0xBE);
            CodeEmitWorks("cp a", 0xBF);
            CodeEmitWorks("cp xh", 0xDD, 0xBC);
            CodeEmitWorks("cp xl", 0xDD, 0xBD);
            CodeEmitWorks("cp yh", 0xFD, 0xBC);
            CodeEmitWorks("cp yl", 0xFD, 0xBD);
        }

        [TestMethod]
        public void AluOpsWithIxIndexedAddressWorkAsExpected()
        {
            CodeEmitWorks("add a,(ix)", 0xDD, 0x86, 0x00);
            CodeEmitWorks("add a,(ix+#0A)", 0xDD, 0x86, 0x0A);
            CodeEmitWorks("add a,(ix-8)", 0xDD, 0x86, 0xF8);
            CodeEmitWorks("adc a,(ix)", 0xDD, 0x8E, 0x00);
            CodeEmitWorks("adc a,(ix+#0A)", 0xDD, 0x8E, 0x0A);
            CodeEmitWorks("adc a,(ix-8)", 0xDD, 0x8E, 0xF8);
            CodeEmitWorks("sbc a,(ix)", 0xDD, 0x9E, 0x00);
            CodeEmitWorks("sbc a,(ix+#0A)", 0xDD, 0x9E, 0x0A);
            CodeEmitWorks("sbc a,(ix-8)", 0xDD, 0x9E, 0xF8);
            CodeEmitWorks("sub (ix)", 0xDD, 0x96, 0x00);
            CodeEmitWorks("sub (ix+#0A)", 0xDD, 0x96, 0x0A);
            CodeEmitWorks("sub (ix-8)", 0xDD, 0x96, 0xF8);
            CodeEmitWorks("and (ix)", 0xDD, 0xA6, 0x00);
            CodeEmitWorks("and (ix+#0A)", 0xDD, 0xA6, 0x0A);
            CodeEmitWorks("and (ix-8)", 0xDD, 0xA6, 0xF8);
            CodeEmitWorks("xor (ix)", 0xDD, 0xAE, 0x00);
            CodeEmitWorks("xor (ix+#0A)", 0xDD, 0xAE, 0x0A);
            CodeEmitWorks("xor (ix-8)", 0xDD, 0xAE, 0xF8);
            CodeEmitWorks("or (ix)", 0xDD, 0xB6, 0x00);
            CodeEmitWorks("or (ix+#0A)", 0xDD, 0xB6, 0x0A);
            CodeEmitWorks("or (ix-8)", 0xDD, 0xB6, 0xF8);
            CodeEmitWorks("cp (ix)", 0xDD, 0xBE, 0x00);
            CodeEmitWorks("cp (ix+#0A)", 0xDD, 0xBE, 0x0A);
            CodeEmitWorks("cp (ix-8)", 0xDD, 0xBE, 0xF8);
        }

        [TestMethod]
        public void AluOpsWithIzIndexedAddressWorkAsExpected()
        {
            CodeEmitWorks("add a,(iy)", 0xFD, 0x86, 0x00);
            CodeEmitWorks("add a,(iy+#0A)", 0xFD, 0x86, 0x0A);
            CodeEmitWorks("add a,(iy-8)", 0xFD, 0x86, 0xF8);
            CodeEmitWorks("adc a,(iy)", 0xFD, 0x8E, 0x00);
            CodeEmitWorks("adc a,(iy+#0A)", 0xFD, 0x8E, 0x0A);
            CodeEmitWorks("adc a,(iy-8)", 0xFD, 0x8E, 0xF8);
            CodeEmitWorks("sbc a,(iy)", 0xFD, 0x9E, 0x00);
            CodeEmitWorks("sbc a,(iy+#0A)", 0xFD, 0x9E, 0x0A);
            CodeEmitWorks("sbc a,(iy-8)", 0xFD, 0x9E, 0xF8);
            CodeEmitWorks("sub (iy)", 0xFD, 0x96, 0x00);
            CodeEmitWorks("sub (iy+#0A)", 0xFD, 0x96, 0x0A);
            CodeEmitWorks("sub (iy-8)", 0xFD, 0x96, 0xF8);
            CodeEmitWorks("and (iy)", 0xFD, 0xA6, 0x00);
            CodeEmitWorks("and (iy+#0A)", 0xFD, 0xA6, 0x0A);
            CodeEmitWorks("and (iy-8)", 0xFD, 0xA6, 0xF8);
            CodeEmitWorks("xor (iy)", 0xFD, 0xAE, 0x00);
            CodeEmitWorks("xor (iy+#0A)", 0xFD, 0xAE, 0x0A);
            CodeEmitWorks("xor (iy-8)", 0xFD, 0xAE, 0xF8);
            CodeEmitWorks("or (iy)", 0xFD, 0xB6, 0x00);
            CodeEmitWorks("or (iy+#0A)", 0xFD, 0xB6, 0x0A);
            CodeEmitWorks("or (iy-8)", 0xFD, 0xB6, 0xF8);
            CodeEmitWorks("cp (iy)", 0xFD, 0xBE, 0x00);
            CodeEmitWorks("cp (iy+#0A)", 0xFD, 0xBE, 0x0A);
            CodeEmitWorks("cp (iy-8)", 0xFD, 0xBE, 0xF8);
        }

        [TestMethod]
        public void AluOpsWithExpressionWorkAsExpected()
        {
            CodeEmitWorks("add a,2+#0A*4", 0xC6, 0x2A);
            CodeEmitWorks("adc a,2+#0A*4", 0xCE, 0x2A);
            CodeEmitWorks("sbc a,2+#0A*4", 0xDE, 0x2A);
            CodeEmitWorks("sub 2+#0A*4", 0xD6, 0x2A);
            CodeEmitWorks("and 2+#0A*4", 0xE6, 0x2A);
            CodeEmitWorks("xor 2+#0A*4", 0xEE, 0x2A);
            CodeEmitWorks("or 2+#0A*4", 0xF6, 0x2A);
            CodeEmitWorks("cp 2+#0A*4", 0xFE, 0x2A);
        }

        [TestMethod]
        public void AluOpsWith16BitRegistersWorkAsExpected()
        {
            CodeEmitWorks("add hl,bc", 0x09);
            CodeEmitWorks("add hl,de", 0x19);
            CodeEmitWorks("add hl,hl", 0x29);
            CodeEmitWorks("add hl,sp", 0x39);

            CodeEmitWorks("adc hl,bc", 0xED, 0x4A);
            CodeEmitWorks("adc hl,de", 0xED, 0x5A);
            CodeEmitWorks("adc hl,hl", 0xED, 0x6A);
            CodeEmitWorks("adc hl,sp", 0xED, 0x7A);

            CodeEmitWorks("sbc hl,bc", 0xED, 0x42);
            CodeEmitWorks("sbc hl,de", 0xED, 0x52);
            CodeEmitWorks("sbc hl,hl", 0xED, 0x62);
            CodeEmitWorks("sbc hl,sp", 0xED, 0x72);

            CodeEmitWorks("add ix,bc", 0xDD, 0x09);
            CodeEmitWorks("add ix,de", 0xDD, 0x19);
            CodeEmitWorks("add ix,ix", 0xDD, 0x29);
            CodeEmitWorks("add ix,sp", 0xDD, 0x39);

            CodeEmitWorks("add iy,bc", 0xFD, 0x09);
            CodeEmitWorks("add iy,de", 0xFD, 0x19);
            CodeEmitWorks("add iy,iy", 0xFD, 0x29);
            CodeEmitWorks("add iy,sp", 0xFD, 0x39);
        }

        [TestMethod]
        public void InvalidOpsRaiseError()
        {
            CodeRaisesInvalidArgument("add a,(bc)");
            CodeRaisesInvalidArgument("add a,(de)");
            CodeRaisesInvalidArgument("adc a,(bc)");
            CodeRaisesInvalidArgument("adc a,(de)");
            CodeRaisesInvalidArgument("sbc a,(bc)");
            CodeRaisesInvalidArgument("sbc a,(de)");
            CodeRaisesInvalidArgument("sub (bc)");
            CodeRaisesInvalidArgument("sub (de)");
            CodeRaisesInvalidArgument("and (bc)");
            CodeRaisesInvalidArgument("and (de)");
            CodeRaisesInvalidArgument("xor (bc)");
            CodeRaisesInvalidArgument("xor (de)");
            CodeRaisesInvalidArgument("or (bc)");
            CodeRaisesInvalidArgument("or (de)");
            CodeRaisesInvalidArgument("cp (bc)");
            CodeRaisesInvalidArgument("cp (de)");
        }


        [TestMethod]
        public void InvalidAddIxRaisesError()
        {
            CodeRaisesInvalidArgument("add ix,hl");
            CodeRaisesInvalidArgument("add ix,iy");
        }

        [TestMethod]
        public void InvalidAddIyRaisesError()
        {
            CodeRaisesInvalidArgument("add iy,hl");
            CodeRaisesInvalidArgument("add iy,ix");
        }
    }
}