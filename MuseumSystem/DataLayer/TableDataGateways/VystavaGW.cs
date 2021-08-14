using DTO.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.TableDataGateways
{
    public class VystavaGW
    {
        private VystavaGW()
        {
        }

        public static VystavaGW Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ?? (m_Instance = new VystavaGW());
                }
            }
        }

        private static readonly object m_LockObj = new object();
        private static VystavaGW m_Instance;
        private static string path = "vystavy.json";

        public bool Save(VystavaStruct vystava, out string msgErr)
        {
            msgErr = string.Empty;

            // prepare object for serialization
            VystavyObj vobj = new VystavyObj();
            VystavyObj vobjAppend = new VystavyObj();
            vobj.vystavy = new VystavaList();
            vobj.vystavy.vystavaList = new List<VystavaStruct> { vystava };

            //VystavyObj
            if(!Load(out vobjAppend, out msgErr))
            {
                if (vobjAppend == null)
                {
                    // serialization
                    try
                    {
                        //var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(vystava);
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(vobj);
                        // pripsat do souboru k ostatnim vystavam
                        File.AppendAllText(path, jsonString, Encoding.UTF8);
                    }
                    catch (Exception e)
                    {
                        msgErr = e.Message;
                        return false;
                    }
                    return true;
                }
                return false;
            }
            else
            {
                vobjAppend.vystavy.vystavaList.Add(vystava);
                // serialization
                try
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(vobjAppend);
                    // prepsani souboru
                    using (StreamWriter writer = new StreamWriter(path, false))
                    {
                        writer.Write(jsonString);
                    }
                }
                catch (Exception e)
                {
                    msgErr = e.Message;
                    return false;
                }

                return true;
            }
        }

        public bool Save(VystavyObj vystava, out string msgErr)
        {
            msgErr = string.Empty;
            try
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(vystava);
                // prepsani vystavy
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.Write(jsonString);
                }
                return true;
            }
            catch (Exception e)
            {
                msgErr = e.Message;
                return false;
            }
        }

        public bool Load(out VystavyObj vystavyObj, out string msgErr)
        {
            msgErr = string.Empty;
            string readData;
            
            try
            {
                readData = File.ReadAllText(path);
                // deserialization
                vystavyObj = Newtonsoft.Json.JsonConvert.DeserializeObject<VystavyObj>(readData);
                return true;
            }
            catch (Exception e)
            {
                msgErr = e.Message;
                vystavyObj = null;
                return false;
            }
        }
    }
}
