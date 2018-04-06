using System.Reflection;
using System.Xml;

namespace Zeus.Config
{
    /// <summary>
    /// Exension methods for <see cref="XmlWriter"/> class.
    /// </summary>
    internal static class XmlWriterExtension
    {
        #region Methods

        /// <summary>
        /// Sets the private currentState field of a XmlWellFormedWriter to the internal value State.TopLevel (1).
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object wich state has to be modified.</param>
        public static void FakeWrite(this XmlWriter writer)
        {
            writer.GetType().GetField("currentState", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(writer, 1);
        }

        #endregion
    }
}
