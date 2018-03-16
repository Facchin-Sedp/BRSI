using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRSi
{
    public class FtpServer
    {
        private String _ftpAddressURI, _port, _user, _pwd, _ccsid, _ftpType,_ftpPath,_ftpAddress,_ftpSecure,_ftpSecureKey;

        public String ftpAddressURI
        {
            get { return _ftpAddressURI; }
            set { _ftpAddressURI = value; }
        }

        public String ftpAddress
        {
            get { return _ftpAddress; }
            set { _ftpAddress = value; }
        }


        public String port
        {
            get { return _port; }
            set { _port = value; }
        }

        public String user
        {
            get { return _user; }
            set { _user = value; }
        }

        public String pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }

        public String ccsid
        {
            get { return _ccsid; }
            set { _ccsid = value; }
        }

        public String ftpType
        {
            get { return _ftpType; }
            set { _ftpType = value; }
        }

        public String ftpPath
        {
            get { return _ftpPath; }
            set { _ftpPath = value; }
        }

        public String ftpSecure
        {
            get { return _ftpSecure; }
            set { _ftpSecure = value; }
        }

        public String ftpSecureKey
        {
            get { return _ftpSecureKey; }
            set { _ftpSecureKey = value; }
        }
    }
}
