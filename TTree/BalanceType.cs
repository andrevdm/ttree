using System;

namespace TTree
{
    [Flags]
    public enum BalanceType
    {
        None = 0,
        StopAfterFirstRotate = 1,
        StopAfterEvenBalanceFound = 2
    }
}
