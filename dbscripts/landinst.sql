/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50703
Source Host           : localhost:3306
Source Database       : landinst

Target Server Type    : MYSQL
Target Server Version : 50703
File Encoding         : 65001

Date: 2014-09-09 19:27:41
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for checklog
-- ----------------------------
DROP TABLE IF EXISTS `checklog`;
CREATE TABLE `checklog` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `InfoID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `CheckType` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  `Result` bit(1) DEFAULT NULL,
  `Note` varchar(255) DEFAULT NULL,
  `Data` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `ix_userid` (`UserID`),
  KEY `ix_infoId` (`InfoID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

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
  `Address` varchar(255) DEFAULT NULL,
  `Summary` varchar(1024) DEFAULT NULL,
  `Subjects` varchar(1024) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for examresult
-- ----------------------------
DROP TABLE IF EXISTS `examresult`;
CREATE TABLE `examresult` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ExamID` int(11) NOT NULL,
  `MemberID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `Subjects` varchar(255) DEFAULT NULL,
  `Scores` varchar(255) DEFAULT '0',
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `ix_userid` (`MemberID`),
  KEY `ix_examid` (`ExamID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for examsubject
-- ----------------------------
DROP TABLE IF EXISTS `examsubject`;
CREATE TABLE `examsubject` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `TotalScore` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=gb2312;

-- ----------------------------
-- Table structure for institution
-- ----------------------------
DROP TABLE IF EXISTS `institution`;
CREATE TABLE `institution` (
  `ID` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Status` smallint(6) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `RegistrationNo` varchar(45) DEFAULT NULL,
  `City` varchar(45) DEFAULT NULL,
  `LegalPerson` varchar(45) DEFAULT NULL,
  `MobilePhone` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `ix_inst_name` (`Name`),
  KEY `ix_city` (`City`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for member
-- ----------------------------
DROP TABLE IF EXISTS `member`;
CREATE TABLE `member` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `InstitutionID` int(11) NOT NULL DEFAULT '0',
  `RealName` varchar(255) DEFAULT NULL,
  `IDNo` varchar(255) DEFAULT NULL,
  `Birthday` datetime DEFAULT NULL,
  `Gender` varchar(20) DEFAULT NULL,
  `Status` int(11) DEFAULT '1',
  `Major` int(11) NOT NULL DEFAULT '0',
  `EduRecord` int(11) NOT NULL DEFAULT '0',
  `ProfessionalLevel` int(11) NOT NULL DEFAULT '0',
  `MobilePhone` varchar(127) DEFAULT NULL,
  `Email` varchar(127) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `ix_realname` (`RealName`),
  KEY `ix_inst_id` (`InstitutionID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for profile
-- ----------------------------
DROP TABLE IF EXISTS `profile`;
CREATE TABLE `profile` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  `Data` blob,
  `CheckResult` int(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `ix_userid` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=gb2312;

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(128) NOT NULL,
  `Password` varchar(128) NOT NULL,
  `Question` varchar(255) DEFAULT NULL,
  `Answer` varchar(255) DEFAULT NULL,
  `RegisterTime` datetime NOT NULL,
  `LastLoginTime` datetime DEFAULT NULL,
  `LastLoginIP` varchar(20) DEFAULT NULL,
  `LoginTimes` int(11) NOT NULL DEFAULT '0',
  `Role` int(11) NOT NULL,
  `Deleted` bit(1) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `ix_username` (`Username`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8;

-- ----------------------------
-- View structure for vcheck_inst
-- ----------------------------
DROP VIEW IF EXISTS `vcheck_inst`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER  VIEW `vcheck_inst` AS SELECT
checklog.ID AS ID,
checklog.InfoID AS InfoID,
checklog.UserID AS UserID,
checklog.CheckType AS CheckType,
checklog.UpdateTime AS UpdateTime,
checklog.Result AS Result,
checklog.CreateTime AS CreateTime,
institution.Name AS InstName,
institution.`Status` AS `Status`,
institution.LegalPerson AS LegalPerson,
institution.City AS City,
institution.RegistrationNo AS RegistrationNo,
checklog.`Data`
from (`checklog` join `institution` on((`checklog`.`UserID` = `institution`.`ID`))) ;

-- ----------------------------
-- View structure for vcheck_member
-- ----------------------------
DROP VIEW IF EXISTS `vcheck_member`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER  VIEW `vcheck_member` AS SELECT
checklog.ID AS ID,
checklog.InfoID AS InfoID,
checklog.UserID AS UserID,
checklog.CheckType AS CheckType,
checklog.UpdateTime AS UpdateTime,
checklog.Result AS Result,
checklog.CreateTime AS CreateTime,
member.RealName AS RealName,
member.`Status` AS `Status`,
member.Gender AS Gender,
member.InstitutionID AS InstitutionID,
member.Major AS Major,
member.EduRecord AS EduRecord,
checklog.`Data` AS `Data`,
member.Birthday,
member.MobilePhone
from (`checklog` join `member` on((`checklog`.`UserID` = `member`.`ID`))) ;

-- ----------------------------
-- View structure for vexamresult
-- ----------------------------
DROP VIEW IF EXISTS `vexamresult`;
CREATE ALGORITHM=UNDEFINED DEFINER=`loowootech`@`localhost` SQL SECURITY DEFINER  VIEW `vexamresult` AS select `examresult`.`ID` AS `ID`,`examresult`.`Subjects` AS `Subjects`,`examresult`.`MemberID` AS `MemberID`,`examresult`.`ExamID` AS `ExamID`,`member`.`RealName` AS `RealName`,`member`.`InstitutionID` AS `InstitutionID`,`examresult`.`Scores` AS `Scores`,`member`.`Status` AS `Status`,`examresult`.`CreateTime` AS `CreateTime` from (`examresult` join `member` on((`examresult`.`ExamID` = `member`.`ID`))) ;


INSERT INTO `user` VALUES ('1', 'admin', '21232f297a57a5a743894a0e4a801fc3', '你喜欢的宠物叫什么名字？', null, '2014-07-22 20:43:58', '2014-09-09 19:10:57', '::1', '11', '3', '\0');
