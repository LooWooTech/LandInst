/*
MySQL Data Transfer
Source Host: localhost
Source Database: landinst
Target Host: localhost
Target Database: landinst
Date: 2014/8/4 10:24:23
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
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;

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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for examresult
-- ----------------------------
DROP TABLE IF EXISTS `examresult`;
CREATE TABLE `examresult` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `MemberID` int(11) NOT NULL,
  `ExamID` int(11) NOT NULL,
  `Result` bit(1) DEFAULT NULL,
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

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
  `Gender` varchar(20) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `MobilePhone` varchar(255) DEFAULT NULL,
  `Status` int(11) DEFAULT '1',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for practice
-- ----------------------------
DROP TABLE IF EXISTS `practice`;
CREATE TABLE `practice` (
  `MemberID` int(11) NOT NULL,
  `Data` blob,
  PRIMARY KEY (`MemberID`)
) ENGINE=InnoDB DEFAULT CHARSET=gb2312;

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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=gb2312;

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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=gb2312;

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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- View structure for vcheck_inst
-- ----------------------------
DROP VIEW IF EXISTS `vcheck_inst`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vcheck_inst` AS select `checklog`.`ID` AS `ID`,`checklog`.`InfoID` AS `InfoID`,`checklog`.`UserID` AS `UserID`,`checklog`.`CheckType` AS `CheckType`,`checklog`.`UpdateTime` AS `UpdateTime`,`checklog`.`Result` AS `Result`,`checklog`.`CreateTime` AS `CreateTime`,`institution`.`Name` AS `InstName`,`institution`.`Type` AS `Type`,`institution`.`Status` AS `Status`,`institution`.`LegalRepresentative` AS `LegalRepresentative`,`institution`.`MobilePhone` AS `MobilePhone`,`institution`.`City` AS `City`,`institution`.`RegistrationNo` AS `RegistrationNo`,`institution`.`Fullname` AS `Fullname` from (`checklog` join `institution` on((`checklog`.`UserID` = `institution`.`ID`)));

-- ----------------------------
-- View structure for vcheck_member
-- ----------------------------
DROP VIEW IF EXISTS `vcheck_member`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vcheck_member` AS select `checklog`.`ID` AS `ID`,`checklog`.`InfoID` AS `InfoID`,`checklog`.`UserID` AS `UserID`,`checklog`.`CheckType` AS `CheckType`,`checklog`.`UpdateTime` AS `UpdateTime`,`checklog`.`Result` AS `Result`,`checklog`.`CreateTime` AS `CreateTime`,`member`.`RealName` AS `RealName`,`member`.`Status` AS `Status`,`member`.`Gender` AS `Gender`,`member`.`MobilePhone` AS `MobilePhone`,`member`.`InstitutionID` AS `InstitutionID`,`user`.`Username` AS `Username` from ((`checklog` join `member` on((`checklog`.`UserID` = `member`.`ID`))) join `user` on((`checklog`.`UserID` = `user`.`ID`)));

-- ----------------------------
-- View structure for vcheck_transfer
-- ----------------------------
DROP VIEW IF EXISTS `vcheck_transfer`;
CREATE ALGORITHM=UNDEFINED DEFINER=`loowootech`@`localhost` SQL SECURITY DEFINER VIEW `vcheck_transfer` AS select `checklog`.`ID` AS `ID`,`checklog`.`InfoID` AS `InfoID`,`checklog`.`UserID` AS `UserID`,`checklog`.`CheckType` AS `CheckType`,`checklog`.`CreateTime` AS `CreateTime`,`checklog`.`UpdateTime` AS `UpdateTime`,`checklog`.`Result` AS `Result`,`transfer`.`TargetInstID` AS `TargetInstID`,`transfer`.`Mode` AS `Mode`,`member`.`RealName` AS `RealName`,`transfer`.`CurrentInstID` AS `CurrentInstID` from ((`checklog` join `transfer` on((`checklog`.`InfoID` = `transfer`.`ID`))) join `member` on((`checklog`.`UserID` = `member`.`ID`))) where (`checklog`.`CheckType` = '2');

-- ----------------------------
-- View structure for vmember
-- ----------------------------
DROP VIEW IF EXISTS `vmember`;
CREATE ALGORITHM=UNDEFINED DEFINER=`loowootech`@`localhost` SQL SECURITY DEFINER VIEW `vmember` AS select `user`.`Username` AS `Username`,`user`.`ID` AS `ID`,`member`.`RealName` AS `RealName`,`member`.`InstitutionID` AS `InstitutionID`,`member`.`IDNo` AS `IDNo`,`member`.`Birthday` AS `Birthday`,`member`.`Gender` AS `Gender`,`member`.`Email` AS `Email`,`member`.`MobilePhone` AS `MobilePhone`,`member`.`Status` AS `Status`,`institution`.`Name` AS `InstitutionName`,`user`.`Deleted` AS `Deleted`,`user`.`RegisterTime` AS `RegisterTime`,`user`.`LastLoginTime` AS `LastLoginTime`,`user`.`Role` AS `Role` from ((`user` join `member` on((`user`.`ID` = `member`.`ID`))) join `institution` on((`member`.`InstitutionID` = `institution`.`ID`)));

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `annualcheck` VALUES ('2', '2014-07-05 00:00:00', '2014-09-13 00:00:00', '测试年审', '这次年审比较严格， 请各单位保证资料齐备、正确。');
INSERT INTO `checklog` VALUES ('1', '2', '4', '1', '2014-07-26 15:40:51', '2014-07-26 16:42:16', '', '123');
INSERT INTO `checklog` VALUES ('4', '5', '4', '1', '2014-07-27 14:10:45', null, null, null);
INSERT INTO `checklog` VALUES ('5', '2', '5', '6', '2014-07-27 18:13:01', '2014-07-28 12:55:28', '', null);
INSERT INTO `checklog` VALUES ('6', '1', '5', '5', '2014-07-28 13:26:15', '2014-07-28 13:37:20', '', null);
INSERT INTO `checklog` VALUES ('7', '1', '5', '2', '2014-07-28 18:57:49', '2014-07-29 10:20:39', '', null);
INSERT INTO `checklog` VALUES ('8', '2', '5', '2', '2014-07-29 10:25:31', '2014-07-29 10:25:51', '', null);
INSERT INTO `checklog` VALUES ('9', '3', '5', '2', '2014-07-29 10:28:02', '2014-07-29 10:28:35', '', null);
INSERT INTO `checklog` VALUES ('10', '4', '5', '2', '2014-07-29 10:29:42', null, null, null);
INSERT INTO `checklog` VALUES ('13', '5', '5', '3', '2014-07-29 15:56:46', null, null, null);
INSERT INTO `education` VALUES ('1', '测试继续教育1', '2014-07-18 00:00:00', '2014-10-05 00:00:00', '111', '测试机构', null);
INSERT INTO `exam` VALUES ('2', '2014-07-01 00:00:00', '2014-07-31 00:00:00', '2014-09-10 00:00:00', '2014-07-12 00:00:00', 'Test', null, '上海');
INSERT INTO `examresult` VALUES ('1', '5', '2', '', '2014-07-27 14:33:00');
INSERT INTO `institution` VALUES ('4', 'Institution NO1', '1', '1', '2014-07-26 15:38:44', null, null, null, null, null);
INSERT INTO `member` VALUES ('5', '郑良俊', '4', '32032219821027003X', '1982-10-27 00:00:00', '男', '3667144@qq.com', '18121783230', '1');
INSERT INTO `practice` VALUES ('5', 0x7B22506572736F6E616C5265636F7264734E4F223A22616161222C22536F6369616C53656375726974794E4F223A226363222C22506572736F6E616C5265636F726473496E737469747574696F6E223A226262222C22536F6369616C5365637572697479496E737469747574696F6E223A226464222C225072616374696365526567697374726174696F6E4E4F223A226565222C2243657274696669636174696F6E4E4F223A226666222C2243657274696669636174696F6E73223A5B7B224944223A2265363832643038302D306133392D346537372D626138652D353565636334653830396463222C224E616D65223A223131222C2243657274696669636174696F6E4E6F223A223132222C2243657274696669636174696F6E4C6576656C223A223133227D2C7B224944223A2264646462636237372D356338312D343031622D396336302D666631623835343663393263222C224E616D65223A223231222C2243657274696669636174696F6E4E6F223A223232222C2243657274696669636174696F6E4C6576656C223A223233227D5D2C22426567696E576F726B696E6744617465223A22303030312D30312D30315430303A30303A3030227D);
INSERT INTO `profile` VALUES ('2', '4', '2014-07-26 15:40:46', null, 0x7B22526567697374657265644361706974616C223A22313131222C22427573696E65737353636F7065223A22312D3139222C2243657274696669636174654E6F223A22313131222C22546F74616C50726F66697473223A2231222C2241646472657373223A22313233222C22506F7374636F6465223A22313233222C22456D61696C223A22313233222C2257656273697465223A6E756C6C2C224F7065726174696E67506572696F64223A6E756C6C2C225368617265486F6C64657273223A5B5D2C2243657274696669636174696F6E73223A5B5D2C224944223A302C22537461747573223A302C2243726561746554696D65223A22323031342D30372D32365431353A34303A33312E353238323136392B30383A3030222C2254797065223A302C2246756C6C4E616D65223A22496E737469747574696F6E204E4F31222C224E616D65223A22496E737469747574696F6E204E4F31222C22526567697374726174696F6E4E6F223A2231222C2243697479223A22313233222C224D6F62696C6550686F6E65223A22313233222C224C6567616C526570726573656E746174697665223A22E5BCA0E69F90227D);
INSERT INTO `profile` VALUES ('5', '4', '2014-07-27 14:10:44', '2014-07-27 14:16:08', 0x7B22526567697374657265644361706974616C223A22313131222C22427573696E65737353636F7065223A22312D3139222C2243657274696669636174654E6F223A22313131222C22546F74616C50726F66697473223A2231222C2241646472657373223A22313233222C22506F7374636F6465223A22313233222C22456D61696C223A22313233222C2257656273697465223A6E756C6C2C224F7065726174696E67506572696F64223A6E756C6C2C225368617265486F6C64657273223A5B7B224944223A2232656662653139652D643265662D343461382D393364312D366164643666313535643032222C224E616D65223A22E882A1E4B89C41222C2247656E646572223A22E794B7222C224269727468646179223A6E756C6C2C22536861726573223A22313525222C224D6F62696C65223A6E756C6C7D2C7B224944223A2237663638623733392D353230652D346662612D616537342D613534353930373666376537222C224E616D65223A22E882A1E4B89C42222C2247656E646572223A22E5A5B3222C224269727468646179223A6E756C6C2C22536861726573223A6E756C6C2C224D6F62696C65223A6E756C6C7D5D2C2243657274696669636174696F6E73223A5B5D2C224944223A302C22537461747573223A302C2243726561746554696D65223A22323031342D30372D32365431353A34303A33312E353238323136392B30383A3030222C2254797065223A302C2246756C6C4E616D65223A22496E737469747574696F6E204E4F31222C224E616D65223A22496E737469747574696F6E204E4F31222C22526567697374726174696F6E4E6F223A2231222C2243697479223A22313233222C224D6F62696C6550686F6E65223A22313233222C224C6567616C526570726573656E746174697665223A22E5BCA0E69F90227D);
INSERT INTO `profile` VALUES ('7', '5', '2014-07-27 18:13:23', null, 0x7B225469746C65223A22E8BDAFE4BBB6E5B7A5E7A88BE5B888222C225363686F6F6C223A22E8A5BFE58D97E5A4A7E5ADA6222C224D616A6F72223A22E8AEA1E7AE97E69CBAE8BDAFE4BBB6E58F8AE5BA94E794A8222C224564755265636F7264223A22E5A4A7E4B893222C224564754C6576656C223A22E697A0222C22506F6C69746963616C5374617465223A22E6B885E799BD222C224E6174696F6E616C697479223A22E6B189222C224E6174697665506C616365223A22E6B19FE88B8FE6B29BE58EBF222C22506F7374636F6465223A22323231363030222C2241646472657373223A22E6B19FE88B8FE6B29BE58EBF222C224944223A352C22496E737469747574696F6E4944223A302C2249444E6F223A22333230333232313938323130323730303358222C225265616C4E616D65223A22E98391E889AFE4BF8A222C224269727468646179223A22313938322D31302D32375430303A30303A3030222C2247656E646572223A22E794B7222C22456D61696C223A22333636373134344071712E636F6D222C224D6F62696C6550686F6E65223A223138313231373833323330222C22537461747573223A307D);
INSERT INTO `transfer` VALUES ('1', '5', '0', '4', '0001-01-01 00:00:00', '0', '2014-07-29 10:20:39');
INSERT INTO `transfer` VALUES ('2', '5', '4', '0', '2014-07-29 10:25:31', '1', '2014-07-29 10:25:51');
INSERT INTO `transfer` VALUES ('3', '5', '4', '4', '2014-07-29 10:28:02', '0', '2014-07-29 10:28:35');
INSERT INTO `transfer` VALUES ('4', '5', '4', '0', '2014-07-29 10:29:35', '0', null);
INSERT INTO `user` VALUES ('1', 'admin', '21232f297a57a5a743894a0e4a801fc3', '你喜欢的宠物叫什么名字？', null, '2014-07-22 20:43:58', '2014-07-22 20:43:58', '3', '');
INSERT INTO `user` VALUES ('4', 'inst1', 'e10adc3949ba59abbe56e057f20f883e', 'hi', 'hi', '2014-07-26 15:38:44', '2014-07-26 15:38:44', '2', '');
INSERT INTO `user` VALUES ('5', 'user1', '202cb962ac59075b964b07152d234b70', '你喜欢的宠物叫什么名字？', '123', '2014-07-27 16:08:57', '2014-07-27 16:08:57', '1', '');
