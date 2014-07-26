/*
MySQL Data Transfer
Source Host: localhost
Source Database: landinst
Target Host: localhost
Target Database: landinst
Date: 2014/7/26 16:43:10
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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

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
  `Note` varchar(1024) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

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
  `Summary` varchar(1024) DEFAULT NULL,
  `Address` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for examresult
-- ----------------------------
DROP TABLE IF EXISTS `examresult`;
CREATE TABLE `examresult` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `MemberID` int(11) NOT NULL,
  `ExamID` int(11) NOT NULL,
  `Result` bit(1) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

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
-- Table structure for profile
-- ----------------------------
DROP TABLE IF EXISTS `profile`;
CREATE TABLE `profile` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  `Data` blob,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=gb2312;

-- ----------------------------
-- Table structure for transfer
-- ----------------------------
DROP TABLE IF EXISTS `transfer`;
CREATE TABLE `transfer` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `MemberID` int(11) NOT NULL,
  `CurrentInstID` int(11) NOT NULL,
  `TargetInstID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `Mode` int(1) NOT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=gb2312;

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
  `LastLoginTime` datetime NOT NULL,
  `Role` int(11) NOT NULL,
  `Deleted` bit(1) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- ----------------------------
-- View structure for vcheck_annual
-- ----------------------------
DROP VIEW IF EXISTS `vcheck_annual`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vcheck_annual` AS select `checklog`.`ID` AS `ID`,`checklog`.`InfoID` AS `InfoID`,`checklog`.`CheckType` AS `CheckType`,`checklog`.`UpdateTime` AS `UpdateTime`,`checklog`.`Result` AS `Result`,`checklog`.`CreateTime` AS `CreateTime`,`annualcheck`.`Name` AS `Name`,`institution`.`Name` AS `InstName`,`institution`.`ID` AS `InstID`,`checklog`.`UserID` AS `UserID`,`institution`.`Type` AS `InstType`,`annualcheck`.`ID` AS `AnnualCheckID` from ((`checklog` join `annualcheck` on((`checklog`.`InfoID` = `annualcheck`.`ID`))) join `institution` on((`checklog`.`UserID` = `institution`.`ID`))) where (`checklog`.`CheckType` = '4');

-- ----------------------------
-- View structure for vcheck_education
-- ----------------------------
DROP VIEW IF EXISTS `vcheck_education`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vcheck_education` AS select `checklog`.`ID` AS `ID`,`checklog`.`InfoID` AS `EduID`,`checklog`.`UserID` AS `UserID`,`checklog`.`CheckType` AS `CheckType`,`checklog`.`UpdateTime` AS `UpdateTime`,`checklog`.`Result` AS `Result`,`checklog`.`CreateTime` AS `CreateTime`,`education`.`Name` AS `EduName`,`member`.`RealName` AS `RealName` from ((`checklog` join `education` on((`checklog`.`InfoID` = `education`.`ID`))) join `member` on((`checklog`.`UserID` = `member`.`ID`)));

-- ----------------------------
-- View structure for vcheck_exam
-- ----------------------------
DROP VIEW IF EXISTS `vcheck_exam`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vcheck_exam` AS select `checklog`.`ID` AS `ID`,`checklog`.`InfoID` AS `EduID`,`checklog`.`UserID` AS `UserID`,`checklog`.`CheckType` AS `CheckType`,`checklog`.`UpdateTime` AS `UpdateTime`,`checklog`.`Result` AS `Result`,`checklog`.`CreateTime` AS `CreateTime`,`exam`.`Name` AS `ExamName`,`member`.`RealName` AS `RealName`,`member`.`Status` AS `Status`,`member`.`MobilePhone` AS `MobilePhone` from ((`checklog` join `exam` on((`checklog`.`InfoID` = `exam`.`ID`))) join `member` on((`checklog`.`UserID` = `member`.`ID`)));

-- ----------------------------
-- View structure for vcheck_inst
-- ----------------------------
DROP VIEW IF EXISTS `vcheck_inst`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vcheck_inst` AS select `checklog`.`ID` AS `ID`,`checklog`.`InfoID` AS `InfoID`,`checklog`.`UserID` AS `UserID`,`checklog`.`CheckType` AS `CheckType`,`checklog`.`UpdateTime` AS `UpdateTime`,`checklog`.`Result` AS `Result`,`checklog`.`CreateTime` AS `CreateTime`,`institution`.`Name` AS `InstName`,`institution`.`Type` AS `Type`,`institution`.`Status` AS `Status`,`institution`.`LegalRepresentative` AS `LegalRepresentative`,`institution`.`MobilePhone` AS `MobilePhone`,`institution`.`City` AS `City`,`institution`.`RegistrationNo` AS `RegistrationNo`,`institution`.`Fullname` AS `Fullname` from (`checklog` join `institution` on((`checklog`.`UserID` = `institution`.`ID`)));

-- ----------------------------
-- View structure for vcheck_member
-- ----------------------------
DROP VIEW IF EXISTS `vcheck_member`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vcheck_member` AS select `checklog`.`ID` AS `ID`,`checklog`.`InfoID` AS `InfoID`,`checklog`.`UserID` AS `UserID`,`checklog`.`CheckType` AS `CheckType`,`checklog`.`UpdateTime` AS `UpdateTime`,`checklog`.`Result` AS `Result`,`checklog`.`CreateTime` AS `CreateTime`,`member`.`RealName` AS `RealName`,`member`.`Status` AS `Status`,`member`.`Gender` AS `Gender`,`member`.`MobilePhone` AS `MobilePhone`,`member`.`InstitutionID` AS `InstitutionID`,`user`.`Username` AS `Username` from ((`checklog` join `user` on((`checklog`.`UserID` = `user`.`ID`))) join `member` on((`checklog`.`InfoID` = `member`.`ID`)));

-- ----------------------------
-- View structure for vmember_examresult
-- ----------------------------
DROP VIEW IF EXISTS `vmember_examresult`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vmember_examresult` AS select `examresult`.`ID` AS `ID`,`examresult`.`MemberID` AS `MemberID`,`examresult`.`ExamID` AS `ExamID`,`examresult`.`Result` AS `Result`,`exam`.`Name` AS `ExamName` from (`examresult` join `exam` on((`examresult`.`ExamID` = `exam`.`ID`)));

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `annualcheck` VALUES ('2', '2014-07-05 00:00:00', '2014-09-13 00:00:00', '测试年审', '这次年审比较严格， 请各单位保证资料齐备、正确。');
INSERT INTO `checklog` VALUES ('1', '2', '4', '1', '2014-07-26 15:40:51', '2014-07-26 16:42:16', '', '123');
INSERT INTO `exam` VALUES ('2', '2014-07-01 00:00:00', '2014-07-31 00:00:00', '2014-09-10 00:00:00', '2014-07-12 00:00:00', 'Test', null, '上海');
INSERT INTO `institution` VALUES ('4', 'Institution NO1', '1', '1', '2014-07-26 15:38:44', null, null, null, null, null);
INSERT INTO `profile` VALUES ('2', '4', '2014-07-26 15:40:46', null, 0x7B22526567697374657265644361706974616C223A22313131222C22427573696E65737353636F7065223A22312D3139222C2243657274696669636174654E6F223A22313131222C22546F74616C50726F66697473223A2231222C2241646472657373223A22313233222C22506F7374636F6465223A22313233222C22456D61696C223A22313233222C2257656273697465223A6E756C6C2C224F7065726174696E67506572696F64223A6E756C6C2C225368617265486F6C64657273223A5B5D2C2243657274696669636174696F6E73223A5B5D2C224944223A302C22537461747573223A302C2243726561746554696D65223A22323031342D30372D32365431353A34303A33312E353238323136392B30383A3030222C2254797065223A302C2246756C6C4E616D65223A22496E737469747574696F6E204E4F31222C224E616D65223A22496E737469747574696F6E204E4F31222C22526567697374726174696F6E4E6F223A2231222C2243697479223A22313233222C224D6F62696C6550686F6E65223A22313233222C224C6567616C526570726573656E746174697665223A22E5BCA0E69F90227D);
INSERT INTO `user` VALUES ('1', 'admin', '21232f297a57a5a743894a0e4a801fc3', '你喜欢的宠物叫什么名字？', null, '2014-07-22 20:43:58', '2014-07-22 20:43:58', '3', '');
INSERT INTO `user` VALUES ('4', 'inst1', 'e10adc3949ba59abbe56e057f20f883e', 'hi', 'hi', '2014-07-26 15:38:44', '2014-07-26 15:38:44', '2', '');
