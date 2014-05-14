/*
Navicat MySQL Data Transfer

Source Server         : mysql
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : landinst

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2014-05-14 15:03:37
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for annualcheck
-- ----------------------------
DROP TABLE IF EXISTS `annualcheck`;
CREATE TABLE `annualcheck` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `StartDate` datetime NOT NULL,
  `EndDate` datetime NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Summary` varchar(1024) DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for approval
-- ----------------------------
DROP TABLE IF EXISTS `approval`;
CREATE TABLE `approval` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `InfoID` int(11) NOT NULL,
  `UserID` int(11) DEFAULT NULL,
  `ApprovalType` int(11) NOT NULL,
  `ApprovalTime` datetime DEFAULT NULL,
  `Result` bit(1) DEFAULT NULL,
  `CreateTime` datetime NOT NULL,
  `Note` varchar(1024) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for education
-- ----------------------------
DROP TABLE IF EXISTS `education`;
CREATE TABLE `education` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `StartDate` datetime NOT NULL,
  `EndDate` datetime NOT NULL,
  `Hours` int(11) NOT NULL DEFAULT '0',
  `Agency` varchar(255) DEFAULT NULL,
  `Summary` varchar(512) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for exam
-- ----------------------------
DROP TABLE IF EXISTS `exam`;
CREATE TABLE `exam` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `StartSignDate` datetime NOT NULL,
  `EndSignDate` datetime NOT NULL,
  `StartExamDate` datetime NOT NULL,
  `EndExamDate` datetime NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Summary` varchar(1024) DEFAULT NULL,
  `Address` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for examresult
-- ----------------------------
DROP TABLE IF EXISTS `examresult`;
CREATE TABLE `examresult` (
  `ID` int(11) NOT NULL,
  `MemberID` int(11) NOT NULL,
  `ExamID` int(11) NOT NULL,
  `Result` bit(1) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for infodata
-- ----------------------------
DROP TABLE IF EXISTS `infodata`;
CREATE TABLE `infodata` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `InfoID` int(11) NOT NULL,
  `InfoType` smallint(6) NOT NULL,
  `Status` int(11) NOT NULL,
  `Data` blob,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for institution
-- ----------------------------
DROP TABLE IF EXISTS `institution`;
CREATE TABLE `institution` (
  `ID` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Type` smallint(6) DEFAULT NULL,
  `Status` smallint(6) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `Fullname` varchar(255) DEFAULT NULL,
  `RegistrationNo` varchar(255) DEFAULT NULL,
  `City` varchar(255) DEFAULT NULL,
  `MobilePhone` varchar(255) DEFAULT NULL,
  `LegalRepresentative` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for member
-- ----------------------------
DROP TABLE IF EXISTS `member`;
CREATE TABLE `member` (
  `ID` int(11) NOT NULL,
  `RealName` varchar(255) DEFAULT NULL,
  `InstitutionID` int(11) NOT NULL DEFAULT '0',
  `IDNo` varchar(255) DEFAULT NULL,
  `Birthday` datetime DEFAULT NULL,
  `Gender` int(11) DEFAULT '1',
  `Email` varchar(255) DEFAULT NULL,
  `MobilePhone` varchar(255) DEFAULT NULL,
  `Status` int(11) DEFAULT '1',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(128) NOT NULL,
  `Password` varchar(128) NOT NULL,
  `Question` varchar(255) NOT NULL,
  `Answer` varchar(255) NOT NULL,
  `RegisterTime` datetime NOT NULL,
  `LastLoginTime` datetime NOT NULL,
  `Role` int(11) NOT NULL,
  `Deleted` bit(1) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- View structure for vapproval_annualcheck
-- ----------------------------
DROP VIEW IF EXISTS `vapproval_annualcheck`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER  VIEW `vapproval_annualcheck` AS SELECT
approval.ID,
approval.InfoID,
approval.UserID,
approval.ApprovalType,
approval.ApprovalTime,
approval.Result,
approval.CreateTime,
annualcheck.`Name`,
institution.Fullname,
institution.`Name` AS InstName,
annualcheck.ID AS AnnualCheckID
FROM
approval
INNER JOIN annualcheck ON approval.InfoID = annualcheck.ID
INNER JOIN institution ON approval.UserID = institution.ID ;

-- ----------------------------
-- View structure for vapproval_education
-- ----------------------------
DROP VIEW IF EXISTS `vapproval_education`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER  VIEW `vapproval_education` AS SELECT
approval.ID,
approval.InfoID AS EduID,
approval.UserID,
approval.ApprovalType,
approval.ApprovalTime,
approval.Result,
approval.CreateTime,
education.`Name` AS EduName,
member.RealName
FROM
approval
INNER JOIN education ON approval.InfoID = education.ID
INNER JOIN member ON approval.UserID = member.ID ;

-- ----------------------------
-- View structure for vapproval_exam
-- ----------------------------
DROP VIEW IF EXISTS `vapproval_exam`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER  VIEW `vapproval_exam` AS SELECT
approval.ID,
approval.InfoID AS ExamID,
approval.UserID,
approval.ApprovalType,
approval.ApprovalTime,
approval.Result,
approval.CreateTime,
approval.Note,
exam.`Name` AS ExamName,
member.RealName,
member.`Status`,
member.MobilePhone
FROM
approval
INNER JOIN exam ON approval.InfoID = exam.ID
INNER JOIN member ON approval.UserID = member.ID ;

-- ----------------------------
-- View structure for vapproval_inst
-- ----------------------------
DROP VIEW IF EXISTS `vapproval_inst`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER  VIEW `vapproval_inst` AS SELECT
approval.ID,
approval.InfoID,
approval.UserID,
approval.ApprovalType,
approval.ApprovalTime,
approval.Result,
approval.CreateTime,
institution.`Name` AS InstName,
institution.Type,
institution.`Status`,
institution.LegalRepresentative,
institution.MobilePhone,
institution.City,
institution.Fullname,
institution.RegistrationNo
FROM
approval
INNER JOIN institution ON approval.InfoID = institution.ID ;

-- ----------------------------
-- View structure for vapproval_member
-- ----------------------------
DROP VIEW IF EXISTS `vapproval_member`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER  VIEW `vapproval_member` AS SELECT
approval.ID,
approval.InfoID,
approval.UserID,
approval.ApprovalType,
approval.ApprovalTime,
approval.Result,
approval.CreateTime,
member.RealName,
member.`Status`,
member.Gender,
member.MobilePhone,
member.InstitutionID
FROM
approval
INNER JOIN member ON approval.InfoID = member.ID ;

-- ----------------------------
-- View structure for vmember_examresult
-- ----------------------------
DROP VIEW IF EXISTS `vmember_examresult`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER  VIEW `vmember_examresult` AS SELECT
examresult.ID,
examresult.MemberID,
examresult.ExamID,
examresult.Result,
exam.`Name` AS ExamName
FROM
examresult
INNER JOIN exam ON examresult.ExamID = exam.ID ;
