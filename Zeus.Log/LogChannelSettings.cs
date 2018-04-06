using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Zeus.Config;

namespace Zeus.Log
{
    /// <summary>
    /// This class represents the configuration of a log channel.
    /// Only basic types parameters are allowed.
    /// </summary>
    [SectionName("channel")]
    public sealed class LogChannelSettings : IXmlSerializable
    {
        #region Constants

        /// <summary>
        /// The name of the attribute that contains information about the channel type.
        /// </summary>
        private const string c_TypeAttributeName = "Type";
        /// <summary>
        /// The name of the attribute that contains information about the channel name.
        /// </summary>
        private const string c_NameAttributeName = "Name";
        /// <summary>
        /// The name of the attribute that contains information about the channel minimum log level.
        /// </summary>
        private const string c_MinLvlAttributeName = "MinLvl";
        /// <summary>
        /// The name of the attribute that contains inforamtion about the channel maximus log level.
        /// </summary>
        private const string c_MaxLvlAttributeName = "MaxLvl";
        /// <summary>
        /// The name of the attribute that contains information about the channel message format.
        /// </summary>
        private const string c_FormatAttributeName = "Format";

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize class internal fields.
        /// </summary>
        public LogChannelSettings()
        {
            CustomSettings = new CustomLogChannelSettings();
            MinimumLogLevel = LogLevels.Trace;
            MaximumLogLevel = LogLevels.Fatal;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the log channel type.
        /// </summary>
        public string ChannelType { get; set; }

        /// <summary>
        /// Gets or sets the log channel name.
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// Gets or sets the minimum log level for the log channel.
        /// </summary>
        public LogLevels MinimumLogLevel { get; set; }

        /// <summary>
        /// Gets or sets the maximum log level for the log channel.
        /// </summary>
        public LogLevels MaximumLogLevel { get; set; }

        /// <summary>
        /// Gets or sets the message format of the cahnnel.
        /// </summary>
        public string MessageFormat { get; set; }

        /// <summary>
        /// Gets the channel specific settings.
        /// </summary>
        public CustomLogChannelSettings CustomSettings { get; private set; }

        #endregion

        #region IXmlSerializable interface

        /// <summary>
        /// Gets the XML schema.
        /// </summary>
        /// <returns>The <see cref="XmlSchema"/> object.</returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The reader used to read the XML data.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            //go to first xml node attribute
            reader.MoveToFirstAttribute();
            do
            {
                //check attribute name
                switch (reader.Name)
                {
                    case c_TypeAttributeName:
                        //save channel type
                        ChannelType = reader.Value;
                        break;
                    case c_NameAttributeName:
                        //save channel name
                        ChannelName = reader.Value;
                        break;
                    case c_MinLvlAttributeName:
                        //save minimum log level
                        MinimumLogLevel = (LogLevels)Enum.Parse(typeof(LogLevels), reader.Value, true);
                        break;
                    case c_MaxLvlAttributeName:
                        //save maximum log level
                        MaximumLogLevel = (LogLevels)Enum.Parse(typeof(LogLevels), reader.Value, true);
                        break;
                    case c_FormatAttributeName:
                        //save message format
                        MessageFormat = reader.Value;
                        break;
                    default:
                        //save settings into custom settings
                        CustomSettings.SetValue(reader.Name, reader.Value);
                        break;
                }
            }
            //move to next attribute
            while (reader.MoveToNextAttribute());
            //back to xml element
            reader.MoveToElement();
            //read xml element
            reader.ReadStartElement();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The writer used to write the XML data.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            //write channel type as xml node attribute
            writer.WriteStartAttribute(c_TypeAttributeName);
            writer.WriteValue(ChannelType);
            writer.WriteEndAttribute();
            if (!string.IsNullOrEmpty(ChannelName))
            {
                //write channel name as xml node attribute
                writer.WriteStartAttribute(c_NameAttributeName);
                writer.WriteValue(ChannelName);
                writer.WriteEndAttribute();
            }
            //write minimum log level as xml node attribute
            writer.WriteStartAttribute(c_MinLvlAttributeName);
            writer.WriteValue(MinimumLogLevel.ToString());
            writer.WriteEndAttribute();
            //write maximum log level as xml node attribute
            writer.WriteStartAttribute(c_MaxLvlAttributeName);
            writer.WriteValue(MaximumLogLevel.ToString());
            writer.WriteEndAttribute();
            if (!string.IsNullOrEmpty(MessageFormat))
            {
                //write message format as xml node attribute
                writer.WriteStartAttribute(c_FormatAttributeName);
                writer.WriteValue(MessageFormat);
                writer.WriteEndAttribute();
            }
            //write all remaining custom settings as xml node attributes
            foreach (string key in CustomSettings.GetKeys())
            {
                writer.WriteStartAttribute(key);
                writer.WriteValue(CustomSettings.GetValue<string>(key));
                writer.WriteEndAttribute();
            }
        }

        #endregion
    }
}
