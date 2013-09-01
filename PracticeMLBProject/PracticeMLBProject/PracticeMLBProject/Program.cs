
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//This next mess of code does the same thing as a union in c
//It basically allows multiple variables inside the structure to map to the same locations in memory
//Below, picture an integer and an array of four bytes taking up the same memory location
using System.Runtime.InteropServices;
[StructLayout(LayoutKind.Explicit)]
struct ByteInt
{
    [FieldOffset(0)]    //The integer starts at the begining offset 0, and takes up 4 bytes
    public int i;
    [FieldOffset(0)]    //The first byte also starts at offset 0 and is obviously 1 byte long...
    public byte b0;
    [FieldOffset(1)]    //...followed by the second byte at offset 1, and so on. 
    public byte b1;
    [FieldOffset(2)]
    public byte b2;
    [FieldOffset(3)]
    public byte b3;
}
namespace PracticeMLBProject
{
    enum Format {REGISTER=0, IMMEDIATE,BRANCH, SCALAR, SIMD};

    class Program
    {
        const int REGS_LENGTH = 32;

        //Since this is just a proof of concept, so I'm lazy, but just close your
        //eyes and pretend that this is inside the MLB class!
        static byte[] regs = new byte[REGS_LENGTH];

        static void Main(string[] args)
        {

            //Create a 32 bit integer
            int i1 = 1778332123;  //arbitrary value less than 2^32 that still requires all four bytes of the integer
            Console.Out.WriteLine("\nThe 32 bit value before it gets loaded to the register file is: " + i1);

            //Demonstrate storing and loading all 32 bits
            StoreToRegs(i1, 3, 4);  // store 4 bytes in r3, r4, r5, and r6
            int i1Loaded32Bits = LoadFromRegs(3, 4); 
            Console.Out.WriteLine("\nThe full 32 bit value loaded back from the register file is: " + i1Loaded32Bits);

            //Test loading and storing 24 bits of the 32 bit integer to the register file
            StoreToRegs(i1, 9, 3);  // store 3 bytes in r9, r10, and r11 
            int i1Loaded24Bits = LoadFromRegs(9, 3);
            Console.Out.WriteLine("\nThe first 24 bits loaded back from the register file are: " + i1Loaded24Bits);

            //Test loading and storing 16 bits of the 32 bit integer to the register file
            StoreToRegs(i1, 13, 2);  // store 2 bytes in r13 and r14
            int i1Loaded16Bits = LoadFromRegs(13,2);
            Console.Out.WriteLine("\nThe first 16 bits loaded back from the register file are: " + i1Loaded16Bits);

            //Test loading and storing 16 bits of the 32 bit integer to the register file
            StoreToRegs(i1, 21, 1);  // store 1 byte in r21
            int i1Loaded8Bits = LoadFromRegs(21, 1);
            Console.Out.WriteLine("\nThe first 8 bits loaded back from the register file are: " + i1Loaded8Bits);

            //Print the Register Contents
            PrintRegs();


            Cluster clust = new Cluster();
            clust.CreateCluster();
        }

        //StoreInt32ToRegs(): stores an integer value to an sequence of 4 bytes at an arbitrary register location
        static void StoreToRegs(int intIn, int byteAddress, int widthBytes)
        {
            //Initialize the byte converter struct
            ByteInt converter;
            converter.b0 = 0;
            converter.b1 = 0;
            converter.b2 = 0;
            converter.b3 = 0;

            //store value in the struct as an int 
            converter.i = intIn;

            //If the byte address is valid (given the width in bytes) write the data to the reg file
            if (byteAddress <= REGS_LENGTH - widthBytes)
            {
                regs[byteAddress] = converter.b0;   //always write atleast 1 byte
                //write the second, third, and fourth bytes if the width is two, 3, and 4, respectively
                if (widthBytes > 1)
                    regs[byteAddress + 1] = converter.b1;
                if (widthBytes > 2)
                    regs[byteAddress + 2] = converter.b2;
                if (widthBytes > 3)
                    regs[byteAddress + 3] = converter.b3;
            }
        }

        //LoadInt32FromRegs(): Loads an integer from a sequence of four consecutive bytes in the register file
        static int LoadFromRegs(int byteAddress, int widthBytes)
        {
            ByteInt converter;
            converter.i = 0;

            if (byteAddress <= REGS_LENGTH - widthBytes)  //grab the individual bytes from the reg array and pack them into the struct
            {
                converter.b0 = regs[byteAddress];   //always grab at least one byte
                //grab the second, third and fourth bytes if the width in bytes is 2,3,and 4 respectively
                if (widthBytes > 1)
                    converter.b1 = regs[byteAddress + 1];
                if (widthBytes > 2)
                    converter.b2 = regs[byteAddress + 2];
                if (widthBytes > 3)
                    converter.b3 = regs[byteAddress + 3];
            }

            return converter.i;  //return the data packed into the struct as an integer
        }

        //PrintRegs(): Prints the register file to prove that I'm actually not lying!
        static void PrintRegs()
        {
            Console.Out.WriteLine("\nRegister File Contents:");
            for (int i = 0; i < REGS_LENGTH; i++)
            {
                string line = "\tReg[" + i + "]: " + regs[i];
                Console.Out.WriteLine(line);
            }

        }
    }

    //MLB class skeleton  
    class MLB
    {
        //Othere MLBS in the cluster 
        MLB mlbPlus1;
        MLB mlbPlus2;
        MLB mlbPlus3;
        int mlbClusterIndex;

        //32 byte register file
        const int REGS_LENGTH = 32;
        static byte[] Regs = new byte[REGS_LENGTH];

        //"Scalar" single bit register file
        const int SCAL_REGS_LENGTH = 16;
        static bool[] scalarRegs = new bool[SCAL_REGS_LENGTH];

        //Pocessor Status Register Bits
        bool UserSelected;
        bool CarryOut;
        bool Overflow;
        bool Zero;

        //system variables
        int PC;

        //RegisterClusterLocation():  method called while constructing a cluster to
        //link an mlb to each of its neighbors in the cluster. This will greatly facilitate 
        //the implementation of the intercluster bus, since each mlb will simply know what its neighbors are
        public void RegisterClusterLocation(MLB mlbPlus1, MLB mlbPlus2, MLB mlbPlus3, int mlbClusterIndex)
        {
            this.mlbPlus1 = mlbPlus1;
            this.mlbPlus2 = mlbPlus2;
            this.mlbPlus3 = mlbPlus3;
            this.mlbClusterIndex = mlbClusterIndex;

        }

        //ProcessInstruction(): 
        //Gets the next instruct
        public void ProcessInstruction()
        {
            //MAHA parser would live inside here
        }

        //Here is where the individual isa instructions could go
        public void ADD(RegisterFormat rFmt)
        {
            

        }

        //StoreInt32ToRegs(): stores an integer value to an sequence of 4 bytes at an arbitrary register location
        static void StoreToRegs(int intIn, int byteAddress, int widthBytes)
        {
            //Initialize the byte converter struct
            ByteInt converter;
            converter.b0 = 0;
            converter.b1 = 0;
            converter.b2 = 0;
            converter.b3 = 0;

            //store value in the struct as an int 
            converter.i = intIn;

            //If the byte address is valid (given the width in bytes) write the data to the reg file
            if (byteAddress <= REGS_LENGTH - widthBytes)
            {
                Regs[byteAddress] = converter.b0;   //always write atleast 1 byte
                //write the second, third, and fourth bytes if the width is two, 3, and 4, respectively
                if (widthBytes > 1)
                    Regs[byteAddress + 1] = converter.b1;
                if (widthBytes > 2)
                    Regs[byteAddress + 2] = converter.b2;
                if (widthBytes > 3)
                    Regs[byteAddress + 3] = converter.b3;
            }
        }

        //LoadInt32FromRegs(): Loads an integer from a sequence of four consecutive bytes in the register file
        static int LoadFromRegs(int byteAddress, int widthBytes)
        {
            ByteInt converter;
            converter.i = 0;

            if (byteAddress <= REGS_LENGTH - widthBytes)  //grab the individual bytes from the reg array and pack them into the struct
            {
                converter.b0 = Regs[byteAddress];   //always grab at least one byte
                //grab the second, third and fourth bytes if the width in bytes is 2,3,and 4 respectively
                if (widthBytes > 1)
                    converter.b1 = Regs[byteAddress + 1];
                if (widthBytes > 2)
                    converter.b2 = Regs[byteAddress + 2];
                if (widthBytes > 3)
                    converter.b3 = Regs[byteAddress + 3];
            }

            return converter.i;  //return the data packed into the struct as an integer
        }
        //PrintRegs(): Prints the register file to prove that I'm actually not lying!
        static void PrintRegs()
        {
            Console.Out.WriteLine("\nRegister File Contents:");
            for (int i = 0; i < REGS_LENGTH; i++)
            {
                string line = "\tReg[" + i + "]: " + Regs[i];
                Console.Out.WriteLine(line);
            }

        }
    }

    //Cluster: represets a cluster of 4 MLBS
    class Cluster
    {
        MLB[] mlbs = new MLB[4];

        // Create Cluster(): Creates a cluster by instantiating the mlbs and linking them together. Note that
        //They links are addressed using modulo to allow the neighbor relationships to wrap around (As suggested in the ISA)
        public void CreateCluster()
        {
            for (int i = 0; i < 4; i++)
            {
                mlbs[i] = new MLB();
                mlbs[i].RegisterClusterLocation(mlbs[(i + 1) % 4], mlbs[(i + 2) % 4], mlbs[(i + 3) % 4], i); //links each mlb to each of the other mlbs
            }
            
            Instruction inst = new Instruction();
            

        }
    }

    //Create an array of Instructions at startup.  Each of these instructions is actually a child object.  Use 
    //the format field to detemine what type of child it is.  Then cast it to the correct format so that you can extract the fields.
    //I believe that preparsing the instructions and storing them in such a structure would greatly improve the efficiency of the
    //simulator. There are more elegant ways of doing this than using the base/child class approach and casts, but this is 
    //probably the simplest

    //Note: The opcode and format fields are protected because they are critical in determining what the instruction is
    //Fields in child classes, which contain contain other less critical fields, are generally not private
  
    class Instruction
    {

        protected Format format;
        protected int opCode;
        public int OpCode
        {
            get { return opCode; }
        }
        public Format Format
        {
            get { return format; }
        }
    }
    class RegisterFormat : Instruction
    {
        public bool[] Func;
        public int Rd;
        public int Ra;
        public int Rb;
        int Rc;
        bool[] Cond;
        RegisterFormat(int opCode, bool[] Func, int Rd, int Ra, int Rb, int Rc, bool[] Cond)
        {
           //set generic instruction parameters
            this.opCode = opCode;
            this.format = Format.REGISTER;

            //set the parameters specific to an Register format instruction
            this.Func = Func;
            this.Rd = Rd;
            this.Ra = Ra;
            this.Rb = Rb;
            this.Rc = Rc;
            this.Cond = Cond;
        }
    }
    class ImmediateFormat : Instruction
    {

    }
    class BranchFormat : Instruction
    {

    }
    class ScalarFormat : Instruction
    {

    }
    class SIMDFormat : Instruction
    {

    }
}
