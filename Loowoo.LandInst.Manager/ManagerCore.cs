﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Manager
{
    public class ManagerCore
    {
        public static ManagerCore Instance = new ManagerCore();

        private UserManager _userManager;
        public UserManager UserManager
        {
            get { return _userManager == null ? _userManager = new UserManager() : _userManager; }
        }

        private MemberManager _memberManager;
        public MemberManager MemberManager
        {
            get { return _memberManager == null ? _memberManager = new MemberManager() : _memberManager; }
        }

        private InstitutionManager _institutionManager;
        public InstitutionManager InstitutionManager
        {
            get { return _institutionManager == null ? _institutionManager = new InstitutionManager() : _institutionManager; }
        }

        private ExamManager _examManager;
        public ExamManager ExamManager
        {
            get { return _examManager == null ? _examManager = new ExamManager() : _examManager; }
        }

        private CheckLogManager _checkLogManager;
        public CheckLogManager CheckLogManager
        {
            get { return _checkLogManager == null ? _checkLogManager = new CheckLogManager() : _checkLogManager; }
        }

        private EducationManager _educationManager;
        public EducationManager EducationManager
        {
            get { return _educationManager == null ? _educationManager = new EducationManager() : _educationManager; }
        }


        private AnnualCheckManager _anualCheckManager;
        public AnnualCheckManager AnnualCheckManager
        {
            get { return _anualCheckManager == null ? _anualCheckManager = new AnnualCheckManager() : _anualCheckManager; }
        }

        private ProfileManager _profileManager;
        public ProfileManager ProfileManager
        {
            get { return _profileManager == null ? _profileManager = new ProfileManager() : _profileManager; }
        }


        private FileManager _fileManager;
        public FileManager FileManager
        {
            get { return _fileManager == null ? _fileManager = new FileManager() : _fileManager; }
        }
    }
}
