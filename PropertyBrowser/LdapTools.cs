
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
                System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry(srvr, usr, pwd);
                object nativeObject = entry.NativeObject;


                authenticated = true;
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException cex)
            {
                //not authenticated; reason why is in cex
            }
            catch (System.Exception ex)
            {
                //not authenticated due to some other exception [this is optional]
            }

            return authenticated;
        } // End Function IsAuthenticated


        public static System.DirectoryServices.DirectoryEntry GetDE(string path, bool IntegratedSecurity, string username, string password)
        {
            if (IntegratedSecurity)
                return new System.DirectoryServices.DirectoryEntry(path);

            return new System.DirectoryServices.DirectoryEntry(path, username, password);
        } // End Function GetDE


        public static System.DirectoryServices.DirectoryEntry GetDE(string path)
        {
            return GetDE(path, true, null, null);
        } // End Function GetDE


        public static string GetGroups(string userDn, bool recursive)
        {
            System.Collections.Generic.List<string> groupMemberships = Groups("LDAP://" + userDn, recursive);
            groupMemberships.Sort();


            return string.Join(System.Environment.NewLine, groupMemberships.ToArray());
        } // End Function GetGroups


        public static System.Collections.Generic.List<string> Groups(string userDn, bool recursive)
        {
            System.Collections.Generic.List<string> groupMemberships = new System.Collections.Generic.List<string>();
            return AttributeValuesMultiString("memberOf", userDn, groupMemberships, recursive);
        } // End Function Groups


        // http://stackoverflow.com/questions/45437/determining-members-of-local-groups-via-c-sharp
        public static System.Collections.Generic.List<string> AttributeValuesMultiString(string attributeName, string objectDn
            , System.Collections.Generic.List<string> valuesCollection, bool recursive)
        {
            using (System.DirectoryServices.DirectoryEntry ent = new System.DirectoryServices.DirectoryEntry(objectDn))
            {
                System.DirectoryServices.PropertyValueCollection ValueCollection = ent.Properties[attributeName];
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
                            } // End if (recursive) 

                        } // End if (!valuesCollection.Contains(en.Current.ToString())) 

                    } // End if (en.Current != null) 

                } // Whend

                ent.Close();
                // ent.Dispose();
            } // End Using DirectoryEntry ent

            return valuesCollection;
        } // End Function AttributeValuesMultiString 


    } // End Class LdapTools


} // End Namespace PropertyBrowser
