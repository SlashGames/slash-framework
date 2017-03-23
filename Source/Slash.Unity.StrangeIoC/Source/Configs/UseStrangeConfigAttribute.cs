namespace Slash.Unity.StrangeIoC.Configs
{
    using System;

    public class UseStrangeConfigAttribute : Attribute
    {
        /// <summary>
        ///   Logic to connect module with others.
        /// </summary>
        public Type[] Bridges { get; set; }

        /// <summary>
        ///   Name of domain this config should be used.
        /// </summary>
        public string Domain { get; set; }
    }
}