
using System;
using System.Collections.Generic;
using System.Text;

using System.DirectoryServices;


namespace PropertyBrowser
{


    public class LdapTools
    {


        // http://stackoverflow.com/questions/290548/validate-a-username-and-password-against-active-directory
        public static bool IsAuthenticated(string srvr, string usr, string pwd)
        {
            bool authenticated = false;

            try
            {
                DirectoryEntry entry = new DirectoryEntry(srvr, usr, pwd);
                object nativeObject = entry.NativeObject;


                authenticated = true;
            }
            catch (DirectoryServicesCOMException cex)
            {
                //not authenticated; reason why is in cex
            }
            catch (Exception ex)
            {
                //not authenticated due to some other exception [this is optional]
            }

            return authenticated;
        } // End Function IsAuthenticated


        public static DirectoryEntry GetDE(string path, bool IntegratedSecurity, string username, string password)
        {
            if (IntegratedSecurity)
                return new DirectoryEntry(path);

            return new DirectoryEntry(path, username, password);
        }


        public static DirectoryEntry GetDE(string path)
        {
            return GetDE(path, true, null, null);
        }


        public static string GetGroups(string userDn, bool recursive)
        {
            List<string> groupMemberships = Groups("LDAP://" + userDn, recursive);
            groupMemberships.Sort();


            return string.Join(Environment.NewLine, groupMemberships.ToArray());
        }



        public static List<string> Groups(string userDn, bool recursive)
        {
            List<string> groupMemberships = new List<string>();
            return AttributeValuesMultiString("memberOf", userDn, groupMemberships, recursive);
        }


        // http://stackoverflow.com/questions/45437/determining-members-of-local-groups-via-c-sharp
        public static List<string> AttributeValuesMultiString(string attributeName, string objectDn, List<string> valuesCollection, bool recursive)
        {
            using (DirectoryEntry ent = new DirectoryEntry(objectDn))
            {
                PropertyValueCollection ValueCollection = ent.Properties[attributeName];
                System.Collections.IEnumerator en = ValueCollection.GetEnumerator();

                while (en.MoveNext())
                {
                    if (en.Current != null)
                    {
                        if (!valuesCollection.Contains(en.Current.ToString()))
                        {
                            valuesCollection.Add(en.Current.ToString());
                            if (recursive)
                            {
                                AttributeValuesMultiString(attributeName, "LDAP://" + en.Current.ToString(), valuesCollection, true);
                            }
                        }
                    }
                }

                ent.Close();
                // ent.Dispose();
            } // End Using DirectoryEntry ent

            return valuesCollection;
        }


    }


}
