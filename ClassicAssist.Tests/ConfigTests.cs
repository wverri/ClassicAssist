using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Assistant;
using ClassicAssist.Data;
using ClassicAssist.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace ClassicAssist.Tests
{
    [TestClass]
    public class ConfigTests
    {
        [TestMethod]
        public void WontThrowExceptionOnDeserializeNullConfig()
        {
            TestConfig( null );
        }

        [TestMethod]
        public void WontThrowExceptionOnDeserializeEmptyConfig()
        {
            TestConfig( new JObject() );
        }

        public void TestConfig( JObject json )
        {
            string path = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );

            string profilePath = Path.Combine( path, "Profiles" );

            if (File.Exists( Path.Combine( profilePath, "settings.json" ) ))
            {
                File.Delete( Path.Combine( profilePath, "settings.json" ) );
            }

            IEnumerable<Type> allSettingProvider = Assembly.GetAssembly( typeof( Engine ) ).GetTypes().Where( t => typeof( ISettingProvider ).IsAssignableFrom( t ) && t.IsClass );

            Options options = new Options();

            foreach (Type type in allSettingProvider)
            {
                if (type.IsPublic)
                {
                    ISettingProvider p = (ISettingProvider)Activator.CreateInstance( type );

                    p.Deserialize( json, options );
                }
            }
        }
    }
}