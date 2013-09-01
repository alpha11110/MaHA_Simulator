using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaHA_Simulator
{
    class MLB
    {
        int PC = 0;
        int inst_count = 0;
        int[,] SchedTable = new int[128, 2];
        int[,] RegFile = new int[32,8];
        //No Operation
        void NOP()
        {
            inst_count++;
            PC++;
        }
        //Add, subtract, add immediate, subtract immediate
        int ADD(bool[] func, bool[] cond, int a, int b)
        {
            int d;
            if (func[1] == false)
            {
                d = a + b;
            }
            else
            {
                d = a - b;
            }
            inst_count++;
            PC++;
            return d;
        }
        //Shift or rotate
        int SHF(bool[] func, int a, int b)
        {
            int d;
            //Shift left
            if (func[1] == false && func[0] == false)
            {
                d = a << b;
            }
            //Shift right
            else if (func[1] == false && func[0] == true)
            {
                d = a >> b;
            }
            //Rotate left
            else if (func[1] == true && func[0] == false)
            {

            }
            //Rotate right
            else if (func[1] == true && func[0] == true)
            {

            }
            inst_count++;
            PC++;
            return d;
        }
        //Logical operations
        int LOG(bool[] func, bool[] cond, int a, int b)
        {
            int d = 0;
            //00 = NOT
            if (func[2] == false && func[1] == false)
            {
                d = ~a;
            }
            //01 = XOR
            else if (func[2] == false && func[1] == true)
            {
                d = a ^ b;    
            }
            //10 = OR
            else if (func[2] == true && func[1] == false)
            {
                d = a | b;
            }
            //11 = AND
            else if (func[2] == true && func[1] == true)
            {
                d = a & b;
            }
            inst_count++;
            PC++;
            return d;
        }
        //Lookup table
        int LUT(bool[] func, bool[] imm, int a)
        {
            int d;

            inst_count++;
            PC++;
            return d;
        }
        //Load or store
        int LS(bool[] func, bool[] imm, int a)
        {
            int d;

            inst_count++;
            PC++;
            return d;
        }
        //Move
        int MOV(bool[] func, bool[] imm)
        {
            int d;

            inst_count++;
            PC++;
            return d;
        }
        //Select
        int SEL(bool[] func, bool[] cond, int a, int b, int c)
        {
            int d;
            if (cond[0] == true)
            {
                d = a;
            }
            else if (cond[1] == true)
            {
                d = b;
            }
            else
            {
                d = c;
            }
            inst_count++;
            PC++;
            return d;
        }
        //Branch
        void BR(bool[] func, bool[] cond, int a, int b)
        {

            inst_count++;
        }
        //Fused logic
        int FUSE(bool[] func, bool[] cond, int a, int b, int c)
        {
            int d;

            inst_count++;
            PC++;
            return d;
        }
        //SIMD XOR
        int[] QXOR(int[] a, int[] b)
        {
            int[] d = new int[4];
            d[0] = a[0] ^ b[0];
            d[1] = a[1] ^ b[1];
            d[2] = a[2] ^ b[2];
            d[3] = a[3] ^ b[3];
            inst_count++;
            PC++;
            return d;
        }
        //SIMD Add
        int[] QADD(int[] a, int[] b)
        {
            int[] d = new int[4];
            d[0] = a[0] + b[0];
            d[1] = a[1] + b[1];
            d[2] = a[2] + b[2];
            d[3] = a[3] + b[3];
            inst_count++;
            PC++;
            return d;
        }
        //SIMD Lookup Table
        int[] QLUT(int[] a, int[] b)
        {
            int[] d = new int[4];

            PC++;
            return d;
        }

    }
}
