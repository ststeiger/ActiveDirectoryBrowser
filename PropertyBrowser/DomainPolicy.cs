
// https://github.com/obscuritysystems/PasswordReminder/blob/master/PasswordReminder/DomainPolicy.cs
// https://books.google.ch/books?id=kGApqjobEfsC&pg=PA364&lpg=PA364&dq=class+DomainPolicy&source=bl&ots=p6qwWiVLQ7&sig=MGysdEIPWV9h6fONvRSv0IzsFUI&hl=en&sa=X&redir_esc=y#v=onepage&q=class%20DomainPolicy&f=false

namespace PropertyBrowser
{


    [System.Flags]
    public enum PasswordPolicy
    {
        DOMAIN_PASSWORD_COMPLEX = 1,
        DOMAIN_PASSWORD_NO_ANON_CHANGE = 2,
        DOMAIN_PASSWORD_NO_CLEAR_CHANGE = 4,
        DOMAIN_LOCKOUT_ADMINS = 8,
        DOMAIN_PASSWORD_STORE_CLEARTEXT = 16,
        DOMAIN_REFUSE_PASSWORD_CHANGE = 32
    }


    public class DomainPolicy
    {

        System.DirectoryServices.ResultPropertyCollection attribs;

        public DomainPolicy(System.DirectoryServices.DirectoryEntry domainRoot)
        {
            string[] policyAttributes = new string[] {
                "maxPwdAge", "minPwdAge", "minPwdLength", 
                "lockoutDuration", "lockOutObservationWindow", 
                "lockoutThreshold", "pwdProperties", 
                "pwdHistoryLength", "objectClass", 
                "distinguishedName"
            };

            //we take advantage of the marshaling with
            //DirectorySearcher for LargeInteger values...
            System.DirectoryServices.DirectorySearcher ds = new System.DirectoryServices.DirectorySearcher(domainRoot, "(objectClass=domainDNS)"
                , policyAttributes, System.DirectoryServices.SearchScope.Base
            );
            System.DirectoryServices.SearchResult result = ds.FindOne();

            //do some quick validation...							  
            if (result == null)
            {
                throw new System.ArgumentException("domainRoot is not a domainDNS object.");
            }

            this.attribs = result.Properties;
        }



        public System.TimeSpan LockoutDuration
        {
            get
            {
                string val = "lockoutDuration";
                if (this.attribs.Contains(val))
                {
                    // For some odd reason, the intervals are all stored
                    // as negative numbers. We use this to "invert" them
                    long ticks = System.Math.Abs((long)this.attribs[val][0]);

                    if (ticks > 0)
                        return System.TimeSpan.FromTicks(ticks);
                }

                return System.TimeSpan.MaxValue;
            }
        }

        public System.TimeSpan MaxPasswordAge
        {
            get
            {
                string val = "maxPwdAge";
                if (this.attribs.Contains(val))
                {
                    // For some odd reason, the intervals are all stored
                    // as negative numbers. We use this to "invert" them
                    long ticks = System.Math.Abs((long)this.attribs[val][0]);

                    if (ticks > 0)
                        return System.TimeSpan.FromTicks(ticks);
                }

                return System.TimeSpan.MaxValue;
            }
        }


        public PasswordPolicy PasswordProperties
        {
            get
            {
                string val = "pwdProperties";
                //this should fail if not found
                return (PasswordPolicy)this.attribs[val][0];
            }
        }


    }


}