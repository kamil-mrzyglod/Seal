namespace Seal.Fody
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class LeaveUnsealedAttribute : Attribute
    {
    }
}