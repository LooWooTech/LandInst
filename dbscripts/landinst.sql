/*
MySQL Data Transfer
Source Host: localhost
Source Database: landinst
Target Host: localhost
Target Database: landinst
Date: 2014/8/22 16:28:30
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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;

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
  `RegistrationNo` varchar(255) DEFAULT NULL,
  `City` varchar(255) DEFAULT NULL,
  `LegalPerson` varchar(255) DEFAULT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=gb2312;

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
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8;

-- ----------------------------
-- View structure for vcheck_inst
-- ----------------------------
DROP VIEW IF EXISTS `vcheck_inst`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vcheck_inst` AS select `checklog`.`ID` AS `ID`,`checklog`.`InfoID` AS `InfoID`,`checklog`.`UserID` AS `UserID`,`checklog`.`CheckType` AS `CheckType`,`checklog`.`UpdateTime` AS `UpdateTime`,`checklog`.`Result` AS `Result`,`checklog`.`CreateTime` AS `CreateTime`,`institution`.`Name` AS `InstName`,`institution`.`Status` AS `Status`,`institution`.`LegalPerson` AS `LegalPerson`,`institution`.`City` AS `City`,`institution`.`RegistrationNo` AS `RegistrationNo`,`checklog`.`Data` AS `Data` from (`checklog` join `institution` on((`checklog`.`UserID` = `institution`.`ID`)));

-- ----------------------------
-- View structure for vcheck_member
-- ----------------------------
DROP VIEW IF EXISTS `vcheck_member`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vcheck_member` AS select `checklog`.`ID` AS `ID`,`checklog`.`InfoID` AS `InfoID`,`checklog`.`UserID` AS `UserID`,`checklog`.`CheckType` AS `CheckType`,`checklog`.`UpdateTime` AS `UpdateTime`,`checklog`.`Result` AS `Result`,`checklog`.`CreateTime` AS `CreateTime`,`member`.`RealName` AS `RealName`,`member`.`Status` AS `Status`,`member`.`Gender` AS `Gender`,`member`.`InstitutionID` AS `InstitutionID`,`member`.`Major` AS `Major`,`member`.`EduRecord` AS `EduRecord`,`checklog`.`Data` AS `Data`,`member`.`Birthday` AS `Birthday`,`member`.`MobilePhone` AS `MobilePhone` from (`checklog` join `member` on((`checklog`.`UserID` = `member`.`ID`)));

-- ----------------------------
-- View structure for vexamresult
-- ----------------------------
DROP VIEW IF EXISTS `vexamresult`;
CREATE ALGORITHM=UNDEFINED DEFINER=`loowootech`@`localhost` SQL SECURITY DEFINER VIEW `vexamresult` AS select `examresult`.`ID` AS `ID`,`examresult`.`Subjects` AS `Subjects`,`examresult`.`MemberID` AS `MemberID`,`examresult`.`ExamID` AS `ExamID`,`member`.`RealName` AS `RealName`,`member`.`InstitutionID` AS `InstitutionID`,`examresult`.`Scores` AS `Scores`,`member`.`Status` AS `Status`,`examresult`.`CreateTime` AS `CreateTime` from (`examresult` join `member` on((`examresult`.`ExamID` = `member`.`ID`)));

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `annualcheck` VALUES ('4', '2014-08-01 00:00:00', '2014-09-01 00:00:00', '2014年度审批', null);
INSERT INTO `checklog` VALUES ('1', '1', '26', '1', '2014-08-18 15:58:16', '2014-08-18 16:13:01', '', null, null);
INSERT INTO `checklog` VALUES ('3', '1', '1', '6', '2014-08-19 19:42:31', '2014-08-19 20:03:46', '', null, '语文,英语');
INSERT INTO `checklog` VALUES ('4', '2', '1', '3', '2014-08-20 19:44:58', '2014-08-21 11:30:11', '', null, '2');
INSERT INTO `checklog` VALUES ('5', '1', '2', '6', '2014-08-21 16:15:04', null, null, null, '语文,英语');
INSERT INTO `checklog` VALUES ('6', '4', '26', '1', '2014-08-21 16:41:13', '2014-08-21 16:42:18', '', null, '4');
INSERT INTO `checklog` VALUES ('7', '4', '26', '4', '2014-08-21 17:56:05', null, null, null, '5');
INSERT INTO `exam` VALUES ('1', '2014-08-01 00:00:00', '2014-09-30 00:00:00', '2014-12-15 00:00:00', '2014-12-16 00:00:00', '测试考试', null, null, '语文,英语,计算机');
INSERT INTO `examresult` VALUES ('2', '1', '1', '2014-08-19 20:03:46', '语文,英语', '语文:100,英语:90', '2014-08-19 20:03:58');
INSERT INTO `examsubject` VALUES ('1', '语文', '100');
INSERT INTO `examsubject` VALUES ('2', '数学', '100');
INSERT INTO `examsubject` VALUES ('3', '英语', '100');
INSERT INTO `examsubject` VALUES ('4', '计算机', '100');
INSERT INTO `examsubject` VALUES ('5', '城市规划', '100');
INSERT INTO `institution` VALUES ('26', '杭州智拓房地产土地评估咨询有限公司', '1', '2014-08-18 14:34:39', '330106000000562', '杭州市', '袁洋');
INSERT INTO `member` VALUES ('1', '26', '郑良俊', '32032219821027003X', '1982-10-27 00:00:00', '男', '2', '2', '2', '18121783230', 'zhengliangjun@gmail.com');
INSERT INTO `member` VALUES ('2', '26', '用户1', '32032219821027003X', '1981-01-01 00:00:00', '男', '0', '3', '3', '18121783230', 'zhengliangjun@gmail.com');
INSERT INTO `profile` VALUES ('1', '26', '2014-08-18 15:55:51', '2014-08-18 16:13:02', 0x7B2254617852656769737472794E6F223A22333330313032373730383130323936222C22526567697374726174696F6E496E737469747574696F6E223A22E69DADE5B79EE5B882E5B7A5E59586E8A18CE694BFE7AEA1E79086E5B180E8A5BFE6B996E58886E5B180222C22526567697374657265644361706974616C223A223530E4B887E58583222C22427573696E65737353636F7065223A6E756C6C2C2243657274696669636174654E6F223A6E756C6C2C22546F74616C50726F66697473223A6E756C6C2C2241646472657373223A22E69DADE5B79EE5B882E4B88BE59F8EE58CBAE4B8ADE5B1B1E58C97E8B7AF363332E58FB7E8B68AE983BDE59586E58AA1E5A4A7E58EA632313031E5AEA4222C22506F7374636F6465223A22333130303033222C224164647265737331223A22E69DADE5B79EE5B882E4B88BE59F8EE58CBAE4B8ADE5B1B1E58C97E8B7AF363332E58FB7E8B68AE983BDE59586E58AA1E5A4A7E58EA632313031E5AEA4222C22506F7374636F646531223A22333130303033222C22456D61696C223A6E756C6C2C2257656273697465223A227777772E6C616E64706C616E2E636E222C224C6567616C706572736F6E4E6F223A2241323031313333303134222C22436F6D70616E7954797065223A22E69C89E99990E8B4A3E4BBBBE585ACE58FB8222C22436F6D6D656E644C6576656C223A22E794B2E7BAA7222C22436F6E74616374506572736F6E223A6E756C6C2C22546563684C6561646572223A6E756C6C2C224F666669636541726561223A6E756C6C2C224D6F62696C6550686F6E65223A223133393538313332353434222C22466178223A22303537312D3837363531363335222C2254656C223A22303537312D3837363531363335222C224F7065726174696E67506572696F64223A6E756C6C2C22486173457865717561747572223A66616C73652C224578657175617475724C6576656C223A6E756C6C2C225072616374696365526567697374726174696F6E4E6F223A6E756C6C2C22436F72706F726174654D656D6265724E6F223A6E756C6C2C22546F74616C4D656D62657273223A312C2250726F4D656D62657273223A312C224578706572744D656D62657273223A6E756C6C2C2245737461626C697368656444617465223A22323030352D30322D30325430303A30303A3030222C225368617265486F6C64657273223A5B7B224E616D65223A22E8A281E6B48B222C2247656E646572223A22E794B7222C224269727468646179223A2231393739E5B9B43038E69C883331E697A5222C22536861726573223A2235382E303030222C224D6F62696C65223A6E756C6C2C225469746C65223A22222C2250726F66657373696F6E616C73223A66616C73657D5D2C2245717569706D656E7473223A5B5D2C22536F66747761726573223A5B5D2C224944223A302C22537461747573223A302C2243726561746554696D65223A22323031342D30382D31385431353A35393A34322E393433393136342B30383A3030222C2254797065223A312C224E616D65223A22E69DADE5B79EE699BAE68B93E688BFE59CB0E4BAA7E59C9FE59CB0E8AF84E4BCB0E592A8E8AFA2E69C89E99990E585ACE58FB8222C22526567697374726174696F6E4E6F223A22333330313036303030303030353632222C2243697479223A22E69DADE5B79EE5B882222C224C6567616C506572736F6E223A22E8A281E6B48B227D, '1');
INSERT INTO `profile` VALUES ('2', '1', '2014-08-18 16:20:03', '2014-08-21 11:30:12', 0x7B224D6F62696C6550686F6E65223A223138313231373833323330222C22456D61696C223A227A68656E676C69616E676A756E40676D61696C2E636F6D222C225469746C65223A22E8BDAFE4BBB6E5B7A5E7A88BE5B888222C225363686F6F6C223A22E8A5BFE58D97E5A4A7E5ADA6222C224564754C6576656C223A22E697A0222C2247726164756174696F6E44617465223A6E756C6C2C225374617274576F726B696E6744617465223A6E756C6C2C22576F726B696E675965617273223A302C224A6F62223A6E756C6C2C22506F6C69746963616C5374617465223A22E6B885E799BD222C224E6174696F6E616C697479223A22E6B189E6978F222C224E6174697665506C616365223A22E6B19FE88B8FE6B29BE58EBF222C22506F7374636F6465223A22323231363030222C2241646472657373223A22E6B19FE88B8FE6B29BE58EBF222C2243657274696669636174696F6E73223A5B7B224E616D65223A2232222C2243657274696669636174696F6E4E6F223A2232222C224F627461696E44617465223A22323031342D30382D32315430303A30303A3030222C2243657274696669636174696F6E4C6576656C223A6E756C6C7D5D2C224A6F6273223A5B7B22537461727444617465223A22323031342F382F3231222C22456E6444617465223A22323031342F382F3231222C22496E737469747574696F6E223A2233222C224F6666696365223A2233222C224E6F7465223A2233227D5D2C22506572736F6E616C5265636F7264734E4F223A2231222C22536F6369616C53656375726974794E4F223A2231222C22506572736F6E616C5265636F726473496E737469747574696F6E223A2231222C22536F6369616C5365637572697479496E737469747574696F6E223A2231222C225072616374696365526567697374726174696F6E4E4F223A6E756C6C2C2243657274696669636174696F6E4E4F223A2231222C224F6666696365223A2231222C2250686F746F31223A222F75706C6F616466696C65732F32362F3633353434313630363734373134333230382E6A7067222C2250686F746F32223A222F75706C6F616466696C65732F32362F3633353434313630363738393236353631372E6A7067222C224944223A312C22496E737469747574696F6E4944223A32362C22496E737469747574696F6E4E616D65223A6E756C6C2C225265616C4E616D65223A22E98391E889AFE4BF8A222C224269727468646179223A22313938322D31302D32375430303A30303A3030222C2247656E646572223A22E794B7222C224D616A6F72223A322C224564755265636F7264223A322C2249444E6F223A22333230333232313938323130323730303358222C22537461747573223A317D, '1');
INSERT INTO `profile` VALUES ('3', '2', '2014-08-20 17:25:09', '2014-08-21 10:16:10', 0x7B224D6F62696C6550686F6E65223A223138313231373833323330222C22456D61696C223A227A68656E676C69616E676A756E40676D61696C2E636F6D222C225469746C65223A22E8BDAFE4BBB6E5B7A5E7A88BE5B888222C225363686F6F6C223A22E8A5BFE58D97E5A4A7E5ADA6222C224564754C6576656C223A22E697A0222C2247726164756174696F6E44617465223A6E756C6C2C225374617274576F726B696E6744617465223A6E756C6C2C22576F726B696E675965617273223A302C224A6F62223A6E756C6C2C22506F6C69746963616C5374617465223A22E6B885E799BD222C224E6174696F6E616C697479223A22E6B189E6978F222C224E6174697665506C616365223A22E6B19FE88B8FE6B29BE58EBF222C22506F7374636F6465223A22323231363030222C2241646472657373223A22E6B19FE88B8FE6B29BE58EBF222C2243657274696669636174696F6E73223A5B7B224E616D65223A2231222C2243657274696669636174696F6E4E6F223A2231222C224F627461696E44617465223A22323031342D30382D31345430303A30303A3030222C2243657274696669636174696F6E4C6576656C223A6E756C6C7D5D2C224A6F6273223A5B7B22537461727444617465223A22323031342F382F3231222C22456E6444617465223A22323031342F382F3231222C22496E737469747574696F6E223A2232222C224F6666696365223A2232222C224E6F7465223A2231227D5D2C22506572736F6E616C5265636F7264734E4F223A6E756C6C2C22536F6369616C53656375726974794E4F223A6E756C6C2C22506572736F6E616C5265636F726473496E737469747574696F6E223A6E756C6C2C22536F6369616C5365637572697479496E737469747574696F6E223A6E756C6C2C225072616374696365526567697374726174696F6E4E4F223A6E756C6C2C2243657274696669636174696F6E4E4F223A6E756C6C2C224F6666696365223A6E756C6C2C2250686F746F31223A222F75706C6F616466696C65732F32362F3633353434323132393531383434373938352E706E67222C2250686F746F32223A222F75706C6F616466696C65732F32362F3633353434323132393534353734393534372E706E67222C224944223A322C22496E737469747574696F6E4944223A32362C22496E737469747574696F6E4E616D65223A6E756C6C2C225265616C4E616D65223A22E794A8E688B731222C224269727468646179223A22313938312D30312D30315430303A30303A3030222C2247656E646572223A22E794B7222C224D616A6F72223A332C224564755265636F7264223A332C2249444E6F223A22333230333232313938323130323730303358222C22537461747573223A307D, null);
INSERT INTO `profile` VALUES ('4', '26', '2014-08-21 16:41:13', '2014-08-21 16:42:18', 0x7B2254617852656769737472794E6F223A22333330313032373730383130323936222C22526567697374726174696F6E496E737469747574696F6E223A22E69DADE5B79EE5B882E5B7A5E59586E8A18CE694BFE7AEA1E79086E5B180E8A5BFE6B996E58886E5B180222C22526567697374657265644361706974616C223A223530E4B887E58583222C22427573696E65737353636F7065223A6E756C6C2C2243657274696669636174654E6F223A6E756C6C2C22546F74616C50726F66697473223A6E756C6C2C2241646472657373223A22E69DADE5B79EE5B882E4B88BE59F8EE58CBAE4B8ADE5B1B1E58C97E8B7AF363332E58FB7E8B68AE983BDE59586E58AA1E5A4A7E58EA632313031E5AEA4222C22506F7374636F6465223A22333130303033222C224164647265737331223A22E69DADE5B79EE5B882E4B88BE59F8EE58CBAE4B8ADE5B1B1E58C97E8B7AF363332E58FB7E8B68AE983BDE59586E58AA1E5A4A7E58EA632313031E5AEA4222C22506F7374636F646531223A22333130303033222C22456D61696C223A6E756C6C2C2257656273697465223A227777772E6C616E64706C616E2E636E222C224C6567616C706572736F6E4E6F223A2241323031313333303134222C22436F6D70616E7954797065223A22E69C89E99990E8B4A3E4BBBBE585ACE58FB8222C22436F6D6D656E644C6576656C223A22E794B2E7BAA7222C22436F6E74616374506572736F6E223A6E756C6C2C22546563684C6561646572223A6E756C6C2C224F666669636541726561223A6E756C6C2C224D6F62696C6550686F6E65223A223133393538313332353434222C22466178223A22303537312D3837363531363335222C2254656C223A22303537312D3837363531363335222C224F7065726174696E67506572696F64223A6E756C6C2C22486173457865717561747572223A66616C73652C224578657175617475724C6576656C223A6E756C6C2C225072616374696365526567697374726174696F6E4E6F223A6E756C6C2C22436F72706F726174654D656D6265724E6F223A6E756C6C2C22546F74616C4D656D62657273223A312C2250726F4D656D62657273223A312C224578706572744D656D62657273223A6E756C6C2C2245737461626C697368656444617465223A22323030352D30322D30325430303A30303A3030222C225368617265486F6C64657273223A5B7B224E616D65223A22E8A281E6B48B222C2247656E646572223A22E794B7222C224269727468646179223A2231393739E5B9B43038E69C883331E697A5222C22536861726573223A2235382E303030222C224D6F62696C65223A6E756C6C2C225469746C65223A22222C2250726F66657373696F6E616C73223A66616C73657D5D2C2245717569706D656E7473223A5B5D2C22536F66747761726573223A5B5D2C2246696C6573223A5B5D2C224944223A32362C22537461747573223A312C2243726561746554696D65223A22323031342D30382D31385431343A33343A3339222C224E616D65223A22E69DADE5B79EE699BAE68B93E688BFE59CB0E4BAA7E59C9FE59CB0E8AF84E4BCB0E592A8E8AFA2E69C89E99990E585ACE58FB8222C22526567697374726174696F6E4E6F223A22333330313036303030303030353632222C2243697479223A22E69DADE5B79EE5B882222C224C6567616C506572736F6E223A22E8A281E6B48B227D, '1');
INSERT INTO `profile` VALUES ('5', '26', '2014-08-21 17:56:04', '2014-08-21 19:20:39', 0x7B2254617852656769737472794E6F223A22333330313032373730383130323936222C22526567697374726174696F6E496E737469747574696F6E223A22E69DADE5B79EE5B882E5B7A5E59586E8A18CE694BFE7AEA1E79086E5B180E8A5BFE6B996E58886E5B180222C22526567697374657265644361706974616C223A223530E4B887E58583222C22427573696E65737353636F7065223A6E756C6C2C2243657274696669636174654E6F223A6E756C6C2C22546F74616C50726F66697473223A6E756C6C2C2241646472657373223A22E69DADE5B79EE5B882E4B88BE59F8EE58CBAE4B8ADE5B1B1E58C97E8B7AF363332E58FB7E8B68AE983BDE59586E58AA1E5A4A7E58EA632313031E5AEA4222C22506F7374636F6465223A22333130303033222C224164647265737331223A22E69DADE5B79EE5B882E4B88BE59F8EE58CBAE4B8ADE5B1B1E58C97E8B7AF363332E58FB7E8B68AE983BDE59586E58AA1E5A4A7E58EA632313031E5AEA4222C22506F7374636F646531223A22333130303033222C22456D61696C223A6E756C6C2C2257656273697465223A227777772E6C616E64706C616E2E636E222C224C6567616C706572736F6E4E6F223A2241323031313333303134222C22436F6D70616E7954797065223A22E69C89E99990E8B4A3E4BBBBE585ACE58FB8222C22436F6D6D656E644C6576656C223A22E794B2E7BAA7222C22436F6E74616374506572736F6E223A6E756C6C2C22546563684C6561646572223A6E756C6C2C224F666669636541726561223A6E756C6C2C224D6F62696C6550686F6E65223A223133393538313332353434222C22466178223A22303537312D3837363531363335222C2254656C223A22303537312D3837363531363335222C224F7065726174696E67506572696F64223A6E756C6C2C22486173457865717561747572223A66616C73652C224578657175617475724C6576656C223A6E756C6C2C225072616374696365526567697374726174696F6E4E6F223A6E756C6C2C22436F72706F726174654D656D6265724E6F223A6E756C6C2C22546F74616C4D656D62657273223A312C2250726F4D656D62657273223A312C224578706572744D656D62657273223A6E756C6C2C2245737461626C697368656444617465223A22323030352D30322D30325430303A30303A3030222C225368617265486F6C64657273223A5B7B224E616D65223A22E8A281E6B48B222C2247656E646572223A22E794B7222C224269727468646179223A2231393739E5B9B43038E69C883331E697A5222C22536861726573223A2235382E303030222C224D6F62696C65223A6E756C6C2C225469746C65223A22222C2250726F66657373696F6E616C73223A66616C73657D5D2C2245717569706D656E7473223A5B5D2C22536F66747761726573223A5B5D2C2246696C6573223A5B7B2246696C654E616D65223A223131222C224465736372697074696F6E223A223131222C225361766550617468223A222F75706C6F616466696C65732F32362F3633353434323435363238353632353030372E6A7067227D2C7B2246696C654E616D65223A223232222C224465736372697074696F6E223A223232222C225361766550617468223A222F75706C6F616466696C65732F32362F3633353434323435363331363735363738382E6A7067227D5D2C224944223A32362C22537461747573223A312C2243726561746554696D65223A22323031342D30382D31385431343A33343A3339222C224E616D65223A22E69DADE5B79EE699BAE68B93E688BFE59CB0E4BAA7E59C9FE59CB0E8AF84E4BCB0E592A8E8AFA2E69C89E99990E585ACE58FB8222C22526567697374726174696F6E4E6F223A22333330313036303030303030353632222C2243697479223A22E69DADE5B79EE5B882222C224C6567616C506572736F6E223A22E8A281E6B48B227D, null);
INSERT INTO `user` VALUES ('1', 'admin', '21232f297a57a5a743894a0e4a801fc3', '你喜欢的宠物叫什么名字？', null, '2014-07-22 20:43:58', '2014-08-21 16:39:27', '::1', '8', '3', '');
INSERT INTO `user` VALUES ('26', '杭州智拓房地产土地评估咨询有限公司', 'e10adc3949ba59abbe56e057f20f883e', null, null, '2014-08-18 14:34:39', '2014-08-21 10:13:49', '::1', '11', '2', '');
