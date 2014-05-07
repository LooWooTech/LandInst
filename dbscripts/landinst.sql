/*
Navicat MySQL Data Transfer

Source Server         : mysql
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : landinst

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2014-05-07 18:41:17
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for approval
-- ----------------------------
DROP TABLE IF EXISTS `approval`;
CREATE TABLE `approval` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `InfoID` int(11) NOT NULL,
  `ApprovalTime` datetime DEFAULT NULL,
  `Result` bit(1) DEFAULT NULL,
  `Note` varchar(1024) DEFAULT NULL,
  `Type` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

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
  `StartSignTime` datetime NOT NULL,
  `EndSignTime` datetime NOT NULL,
  `StartExamTime` datetime NOT NULL,
  `EndExamTime` datetime NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Summary` varchar(1024) DEFAULT NULL,
  `Address` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

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
-- Table structure for membereducation
-- ----------------------------
DROP TABLE IF EXISTS `membereducation`;
CREATE TABLE `membereducation` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `MemberID` int(11) NOT NULL,
  `EducationID` int(11) NOT NULL,
  `SignUpTime` datetime NOT NULL,
  `ApprovalTime` datetime DEFAULT NULL,
  `Approval` bit(1) NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- View structure for vmember
-- ----------------------------
DROP VIEW IF EXISTS `vmember`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER  VIEW `vmember` AS SELECT
member.ID,
member.RealName,
member.InstitutionID,
member.Email,
member.MobilePhone,
member.`Status`,
member.Gender,
`user`.Username,
institution.`Name` AS InstitutionName
FROM
member
INNER JOIN `user` ON member.ID = `user`.ID
LEFT JOIN institution ON member.InstitutionID = institution.ID ;

-- ----------------------------
-- View structure for vmembereducation
-- ----------------------------
DROP VIEW IF EXISTS `vmembereducation`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost`  VIEW `vmembereducation` AS SELECT
me.ID,
me.MemberID,
me.EducationID,
me.SignUpTime,
me.ApprovalTime,
me.Approval,
m.RealName,
e.Summary,
e.`Name` AS EducationName,
m.InstitutionID
FROM
membereducation AS me
INNER JOIN member AS m ON me.MemberID = m.ID
INNER JOIN education AS e ON me.EducationID = e.ID ;
