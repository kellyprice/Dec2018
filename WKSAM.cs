using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADExplorer
{
    public class WKDSAM
    {

        private string _connectionString;
        private Dictionary<string, string> _connectionRegistry;
        private PrincipalContext _principleContext;

        private const string CS_KEY_CONTEXT_TYPE = "contextType";
        private const string CS_KEY_NAME = "name";
        private const string CS_KEY_CONTAINER = "container";
        private const string CS_KEY_CONTEXT_OPTIONS = "contextOptions";
        private const string CS_KEY_USERNAME = "userName";
        private const string CS_KEY_PASWORD = "password";

        private const string CK_KEY_CONTEXTONLY = "ContextOnly";
        private const string CK_KEY_CONTEXT_NAME = "ContextAndName";
        private const string CK_KEY_CONTEXT_NAME_CONTAINER = "ContextAndNameWithContainer";
        private const string CK_KEY_CONTEXT_NAME_CONTAINER_OPTIONS = "ContextAndNameWithContainerAndOptions";
        private const string CK_KEY_CONTEXT_NAME_CONTAINER_OPTIONS_USER = "ContextAndNameWithContainerAndOptionsAndUserDetails";
        private const string CK_KEY_CONTEXT_NAME_USER = "ContextAndNameWithUserDetails";
        private const string CK_KEY_CONTEXT_NAME_CONTAINER_USER = "ContextAndNameWithUserDetailsAndContainer";

        public WKDSAM(string connectionString)
        {
            _connectionString = connectionString;
            _connectionRegistry = new Dictionary<string, string>();
        }

        private PrincipalContext GetPrincipalContext()
        {

            /*

            ContextType - done
            PrincipalContext(ContextType)

            ContextType, name
            PrincipalContext(ContextType, String)

            ContextType, name, container
            PrincipalContext(ContextType, String, String) 

            ContextType, name, container, ContextOptions
            PrincipalContext(ContextType, String, String, ContextOptions) 

            ContextType, name, container, ContextOptions, userName, password
            PrincipalContext(ContextType, String, String, ContextOptions, String, String) 

            ContextType, name, userName, password
            PrincipalContext(ContextType, String, String, String) 

            ContextType, name, container, userName, password
            PrincipalContext(ContextType, String, String, String, String) 


            ContextType can be:-
                ApplicationDirectory, 
                Domain, 
                Machine
            
            name is:-   When ContextType = ApplicationDirectory Then name of server & port
                        When ContextType = Domain Then name of domain or server
                        When ContextType = Machine Then machine name

            ContextOptions can be:-
                Negotiate
                Sealing
                SecureSocketLayer
                ServerBind
                Signing
                SimpleBind
            

            connectionStringformat = "contextType='';name='';container='';contextOptions='';userName='';password=''"

            e.g.

            connectionStringformat = "contextType='Machine';name='MyMachine';container='';contextOptions='Negotiate';userName='';password=''"

    
            */

            if (_principleContext == null)
            {

                var csparts = _connectionString.Split(';');
                foreach (var part in csparts)
                {
                    var partkv = part.Split(new[] { "='" }, StringSplitOptions.None);
                    _connectionRegistry.Add(partkv[0].ToLower(), partkv[1].Replace("'", ""));
                }

                var ctxType = GetContextType();
                var name = GetConnectionStringValue(CS_KEY_NAME);
                var container = GetConnectionStringValue(CS_KEY_CONTAINER);
                var contextOptions = GetConnectionStringValue(CS_KEY_CONTEXT_OPTIONS);
                var userName = GetConnectionStringValue(CS_KEY_USERNAME);
                var password = GetConnectionStringValue(CS_KEY_PASWORD);

                var ctxOption = GetContextOptions(contextOptions);

                var constructorKey = GetConstructorKey(name, container, contextOptions, userName, password);

                switch (constructorKey)
                {
                    case CK_KEY_CONTEXTONLY:
                        _principleContext = new PrincipalContext(ctxType);
                        break;
                    case CK_KEY_CONTEXT_NAME:
                        _principleContext = new PrincipalContext(ctxType, name);
                        break;
                    case CK_KEY_CONTEXT_NAME_CONTAINER:
                        _principleContext = new PrincipalContext(ctxType, name, container);
                        break;
                    case CK_KEY_CONTEXT_NAME_CONTAINER_OPTIONS:
                        _principleContext = new PrincipalContext(ctxType, name, container, ctxOption);
                        break;
                    case CK_KEY_CONTEXT_NAME_CONTAINER_OPTIONS_USER:
                        _principleContext = new PrincipalContext(ctxType, name, container, ctxOption, userName, password);
                        break;
                    case CK_KEY_CONTEXT_NAME_USER:
                        _principleContext = new PrincipalContext(ctxType, name, userName, password);
                        break;
                    case CK_KEY_CONTEXT_NAME_CONTAINER_USER:
                        _principleContext = new PrincipalContext(ctxType, name, container, userName, password);
                        break;
                }

                if (_principleContext == null)
                {
                    throw new InvalidOperationException("The principal context cannot be set");
                }

            }

            return _principleContext;

        }
        
        private ContextType GetContextType()
        {
            var result = ContextType.Machine;
            if(_connectionRegistry.ContainsKey(CS_KEY_CONTEXT_TYPE.ToLower()))
            {
                switch (_connectionRegistry[CS_KEY_CONTEXT_TYPE.ToLower()].ToLower())
                {
                    case "domain":
                        result = ContextType.Domain;
                        break;
                    case "applicationdirectory":
                        result = ContextType.ApplicationDirectory;
                        break;
                    case "machine":
                        result = ContextType.Machine;
                        break;
                }
            }

            return result;
        }

        private string GetConnectionStringValue(string key)
        {
            if (_connectionRegistry.ContainsKey(key.ToLower()))
            {
                return _connectionRegistry[key.ToLower()];
            }

            return "";
        }
        
        private ContextOptions GetContextOptions(string contextOptions)
        {
            //only single options are supported
            var ctxoParts = contextOptions.Split('|').ToList();
            var result = ContextOptions.Negotiate;
            var firstPass = true;

            foreach (string cxto in ctxoParts) {

                switch (contextOptions.ToLower())
                {
                    case "negotiate":
                        result = firstPass ? ContextOptions.Negotiate : result | ContextOptions.Negotiate;
                        break;
                    case "sealing":
                        result = firstPass ? ContextOptions.Sealing : result | ContextOptions.Sealing;
                        break;
                    case "securesocketlayer":
                        result = firstPass ? ContextOptions.SecureSocketLayer : result | ContextOptions.SecureSocketLayer;
                        break;
                    case "serverbind":
                        result = firstPass ? ContextOptions.ServerBind : result | ContextOptions.ServerBind;
                        break;
                    case "signing":
                        result = firstPass ? ContextOptions.Signing : result | ContextOptions.Signing;
                        break;
                    case "simplebind":
                        result = firstPass ? ContextOptions.SimpleBind : result | ContextOptions.SimpleBind;
                        break;
                }

                firstPass = false;
            }

            return result;
            
        }

        private string GetConstructorKey(string name, string container, string contextOptions, string userName, string password)
        {
            var key = CK_KEY_CONTEXTONLY;

            if (!String.IsNullOrEmpty(name) && 
                String.IsNullOrEmpty(container) && 
                String.IsNullOrEmpty(contextOptions) && 
                String.IsNullOrEmpty(userName) && 
                String.IsNullOrEmpty(password))
            {
                key = CK_KEY_CONTEXT_NAME;
            } else if(!String.IsNullOrEmpty(name) && 
                !String.IsNullOrEmpty(container) && 
                String.IsNullOrEmpty(contextOptions) && 
                String.IsNullOrEmpty(userName) && 
                String.IsNullOrEmpty(password))
            {
                key = CK_KEY_CONTEXT_NAME_CONTAINER;
            } else if (!String.IsNullOrEmpty(name) &&
                !String.IsNullOrEmpty(container) &&
                !String.IsNullOrEmpty(contextOptions) &&
                String.IsNullOrEmpty(userName) &&
                String.IsNullOrEmpty(password))
            {
                key = CK_KEY_CONTEXT_NAME_CONTAINER_OPTIONS;
            } else if (!String.IsNullOrEmpty(name) &&
               !String.IsNullOrEmpty(container) &&
               !String.IsNullOrEmpty(contextOptions) &&
               !String.IsNullOrEmpty(userName) &&
               !String.IsNullOrEmpty(password))
            {
                key = CK_KEY_CONTEXT_NAME_CONTAINER_OPTIONS_USER;
            } else if (!String.IsNullOrEmpty(name) &&
                String.IsNullOrEmpty(container) &&
                String.IsNullOrEmpty(contextOptions) &&
                !String.IsNullOrEmpty(userName) &&
                !String.IsNullOrEmpty(password))
            {
                key = CK_KEY_CONTEXT_NAME_USER;
            } else if (!String.IsNullOrEmpty(name) &&
               !String.IsNullOrEmpty(container) &&
               String.IsNullOrEmpty(contextOptions) &&
               !String.IsNullOrEmpty(userName) &&
               !String.IsNullOrEmpty(password))
            {
                key = CK_KEY_CONTEXT_NAME_CONTAINER_USER;
            }


            return key;
        }

        public bool CanConnect()
        {
            bool result = true;

            try
            {
                var pctx = GetPrincipalContext();
            }
            catch(Exception e)
            {
                ProcessError = e;
                result = false;
            }


            return result;
        }

        public Exception ProcessError { get; set; }

        public UserPrincipal FindUser(string userName)
        {
            var pctx = GetPrincipalContext();
            var up = UserPrincipal.FindByIdentity(pctx, userName);
            return up;
        }

        public List<string[]> ListGroups(UserPrincipal up)
        {

            //Should be able to use up.GetGroups but it always throws an error

            var memberOf = new List<string[]>();
            var grps = ListAllGroups().ToList();
            foreach (var grp in grps)
            {
                try
                {
                    var members = ((GroupPrincipal)grp).Members;
                    foreach (var member in members)
                    {
                        if (member.DistinguishedName == up.DistinguishedName)
                        {
                            var memberData = new string[] { grp.Name, grp.DistinguishedName };
                            memberOf.Add(memberData);
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }

            return memberOf;

        }

        public PrincipalSearchResult<Principal> ListAllGroups()
        {
            var pctx = GetPrincipalContext();
            var grpPrin = new GroupPrincipal(pctx);
            var ps = new PrincipalSearcher();
            ps.QueryFilter = grpPrin;

            var result = ps.FindAll();

            return result;
        }
    }
}
