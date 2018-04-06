using System;

namespace Zeus.Log
{
    /// <summary>
    /// Defines an attribute the allow to create multiple instances of the same <see cref="Channels.ILogChannel"/>.
    /// </summary>
    public class AllowMultipleInstancesAttribute : Attribute
    {
    }
}

