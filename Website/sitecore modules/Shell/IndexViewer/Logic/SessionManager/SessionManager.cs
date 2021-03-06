﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace IndexViewer
{
    public class SessionManager
    {
        #region constants

        private const string _sessionManagerInSession = "SessionManager";

        #endregion constants

        #region constructors

        private SessionManager() { }

        #endregion constructors

        #region static properties (Singelton)

        public static SessionManager Instance
        {
            get 
            {
                SessionManager manager = HttpContext.Current.Session[_sessionManagerInSession] as SessionManager;

                if (manager == null)
                {
                    manager = new SessionManager();
                    HttpContext.Current.Session[_sessionManagerInSession] = manager;
                }

                return manager;
            }
        }

        #endregion static properties

        #region public properties

        public IIndex CurrentIndex 
        {
            get
            {
                return _currentIndex;
            }
            set
            {
                ClearAll();
                _currentIndex = value;
                HttpContext.Current.Session[_sessionManagerInSession] = this;
            }
        }
        private IIndex _currentIndex = null;

        
        public SearchResultCollection SearchResult 
        {
            get
            {
                return _searchResult;
            }
            set
            {
                _searchResult = value;
                HttpContext.Current.Session[_sessionManagerInSession] = this;
            }
        }
        private SearchResultCollection _searchResult = null;


        public int CurrentDocumentNumber
        {
            get
            {
                return _currentDocumentNumber;
            }
            set
            {
                _currentDocumentNumber = value;

                if (_currentDocumentNumber < 0 ||
                    CurrentIndex == null)
                {
                    _currentDocumentNumber = 0;
                }

                if (CurrentIndex != null &&
                    _currentDocumentNumber > CurrentIndex.GetDocumentCount())
                {
                    _currentDocumentNumber = CurrentIndex.GetDocumentCount() - 1;
                }

                HttpContext.Current.Session[_sessionManagerInSession] = this;
            }
        }
        private int _currentDocumentNumber;


        public Exception LastError
        {
            get
            {
                return _lastError;
            }
            set
            {
                _lastError = value;
                HttpContext.Current.Session[_sessionManagerInSession] = this;
            }
        }
        private Exception _lastError;


        #endregion public properties

        #region public methods

        public void ClearAll()
        {
            _currentIndex = null;
            _searchResult = null;
            _currentDocumentNumber = 0;

            HttpContext.Current.Session[_sessionManagerInSession] = null;
        }

        public void ClearCurrentDocumentNumber()
        {
            _currentDocumentNumber = 0;
            HttpContext.Current.Session[_sessionManagerInSession] = this;
        }

        public void ClearSearchResult()
        {
            _searchResult = null;
            HttpContext.Current.Session[_sessionManagerInSession] = this;
        }

        #endregion
    }
}
