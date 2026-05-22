CREATE DATABASE  IF NOT EXISTS `uni_db` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `uni_db`;
-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: uni_db
-- ------------------------------------------------------
-- Server version	8.0.44

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `courses`
--

DROP TABLE IF EXISTS `courses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `courses` (
  `course_id` int NOT NULL AUTO_INCREMENT,
  `department_id` int NOT NULL,
  `course_code` varchar(15) NOT NULL,
  `course_title` varchar(150) NOT NULL,
  `units` tinyint NOT NULL,
  PRIMARY KEY (`course_id`),
  UNIQUE KEY `course_code` (`course_code`),
  KEY `fk_courses_department` (`department_id`),
  CONSTRAINT `fk_courses_department` FOREIGN KEY (`department_id`) REFERENCES `departments` (`department_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `courses`
--

LOCK TABLES `courses` WRITE;
/*!40000 ALTER TABLE `courses` DISABLE KEYS */;
INSERT INTO `courses` VALUES (1,1,'CS101','Introduction to Programming',3),(2,1,'CS201','Data Structures and Algorithms',3),(3,2,'IT110','Networking Fundamentals',3),(4,3,'IS120','Business Process Management',3),(5,4,'ENG150','Engineering Mechanics',3),(6,5,'BUS101','Principles of Management',3),(7,8,'MATH101','College Algebra',3),(8,8,'MATH201','Calculus I',4),(9,9,'BIO101','General Biology',4),(10,10,'CHEM101','General Chemistry',4);
/*!40000 ALTER TABLE `courses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `departments`
--

DROP TABLE IF EXISTS `departments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `departments` (
  `department_id` int NOT NULL AUTO_INCREMENT,
  `department_code` varchar(10) NOT NULL,
  `department_name` varchar(120) NOT NULL,
  `departmentYearlySalary` decimal(12,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`department_id`),
  UNIQUE KEY `department_code` (`department_code`),
  UNIQUE KEY `department_name` (`department_name`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `departments`
--

LOCK TABLES `departments` WRITE;
/*!40000 ALTER TABLE `departments` DISABLE KEYS */;
INSERT INTO `departments` VALUES (1,'CS','Computer Science',0.00),(2,'IT','Information Technology',0.00),(3,'IS','Information Systems',0.00),(4,'ENG','Engineering',0.00),(5,'BUS','Business Administration',0.00),(6,'EDU','Education',0.00),(7,'ARTS','Arts and Humanities',0.00),(8,'MATH','Mathematics',0.00),(9,'BIO','Biological Sciences',0.00),(10,'CHEM','Chemistry',0.00);
/*!40000 ALTER TABLE `departments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `enrollments`
--

DROP TABLE IF EXISTS `enrollments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `enrollments` (
  `enrollment_id` int NOT NULL AUTO_INCREMENT,
  `student_id` int NOT NULL,
  `course_id` int NOT NULL,
  `term` varchar(10) NOT NULL,
  `enrolled_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `status` enum('ENROLLED','DROPPED') NOT NULL DEFAULT 'ENROLLED',
  PRIMARY KEY (`enrollment_id`),
  UNIQUE KEY `uq_student_course_term` (`student_id`,`course_id`,`term`),
  KEY `idx_enroll_course_term` (`course_id`,`term`),
  KEY `idx_enroll_student_term` (`student_id`,`term`),
  CONSTRAINT `fk_enroll_course` FOREIGN KEY (`course_id`) REFERENCES `courses` (`course_id`),
  CONSTRAINT `fk_enroll_student` FOREIGN KEY (`student_id`) REFERENCES `students` (`student_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `enrollments`
--

LOCK TABLES `enrollments` WRITE;
/*!40000 ALTER TABLE `enrollments` DISABLE KEYS */;
INSERT INTO `enrollments` VALUES (1,1,1,'2025-1','2026-02-26 23:12:12','ENROLLED'),(2,1,7,'2025-1','2026-02-26 23:12:12','ENROLLED'),(3,2,2,'2025-1','2026-02-26 23:12:12','ENROLLED'),(4,2,3,'2025-1','2026-02-26 23:12:12','ENROLLED'),(5,3,2,'2025-1','2026-02-26 23:12:12','ENROLLED'),(6,4,6,'2025-1','2026-02-26 23:12:12','ENROLLED'),(7,5,4,'2025-1','2026-02-26 23:12:12','ENROLLED'),(8,6,1,'2025-1','2026-02-26 23:12:12','ENROLLED'),(9,8,8,'2025-1','2026-02-26 23:12:12','ENROLLED'),(10,10,9,'2025-1','2026-02-26 23:12:12','ENROLLED');
/*!40000 ALTER TABLE `enrollments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `grades`
--

DROP TABLE IF EXISTS `grades`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `grades` (
  `grade_id` int NOT NULL AUTO_INCREMENT,
  `enrollment_id` int NOT NULL,
  `grade` decimal(4,2) DEFAULT NULL,
  `remarks` enum('PASSED','FAILED','INCOMPLETE','WITHDRAWN') DEFAULT NULL,
  PRIMARY KEY (`grade_id`),
  UNIQUE KEY `enrollment_id` (`enrollment_id`),
  CONSTRAINT `fk_grades_enrollment` FOREIGN KEY (`enrollment_id`) REFERENCES `enrollments` (`enrollment_id`),
  CONSTRAINT `chk_grade_range` CHECK (((`grade` is null) or ((`grade` >= 0.00) and (`grade` <= 100.00))))
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `grades`
--

LOCK TABLES `grades` WRITE;
/*!40000 ALTER TABLE `grades` DISABLE KEYS */;
INSERT INTO `grades` VALUES (1,1,91.50,'PASSED'),(2,2,88.00,'PASSED'),(3,3,79.25,'PASSED'),(4,4,83.00,'PASSED'),(5,5,65.00,'FAILED'),(6,6,90.00,'PASSED'),(7,7,85.75,'PASSED'),(8,8,NULL,'INCOMPLETE'),(9,9,92.00,'PASSED'),(10,10,87.50,'PASSED');
/*!40000 ALTER TABLE `grades` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `professors`
--

DROP TABLE IF EXISTS `professors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `professors` (
  `professorId` int NOT NULL AUTO_INCREMENT,
  `departmentId` int NOT NULL,
  `firstName` varchar(60) NOT NULL,
  `lastName` varchar(60) NOT NULL,
  `monthlySalary` decimal(10,2) NOT NULL,
  `yearlyBonus` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`professorId`),
  KEY `fkProfessorsDepartment` (`departmentId`),
  CONSTRAINT `fkProfessorsDepartment` FOREIGN KEY (`departmentId`) REFERENCES `departments` (`department_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `professors`
--

LOCK TABLES `professors` WRITE;
/*!40000 ALTER TABLE `professors` DISABLE KEYS */;
/*!40000 ALTER TABLE `professors` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `trgProfessorsAfterInsert` AFTER INSERT ON `professors` FOR EACH ROW BEGIN

    -- Purpose:
    -- This trigger runs automatically whenever a new professor record
    -- is inserted into the professors table.
    --
    -- It updates the total yearly salary of the corresponding department.
    --
    -- The formula used is:
    -- (monthlySalary * 12) + yearlyBonus
    --
    -- The calculated amount represents the yearly salary contribution
    -- of the newly inserted professor and is added to the department's
    -- departmentYearlySalary value.

    UPDATE departments
    SET departmentYearlySalary =
        departmentYearlySalary + ((NEW.monthlySalary * 12) + NEW.yearlyBonus)
    WHERE department_id = NEW.departmentId;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `trgProfessorsAfterUpdate` AFTER UPDATE ON `professors` FOR EACH ROW BEGIN

    -- Purpose:
    -- This trigger executes whenever an existing professor record is updated.
    --
    -- It ensures that the departmentYearlySalary remains accurate if:
    -- 1. The professor's salary or yearly bonus changes
    -- 2. The professor is transferred to another department
    --
    -- The trigger subtracts the OLD salary contribution from the
    -- original department and adds the NEW contribution to the
    -- updated department.
    --
    -- Contribution formula:
    -- (monthlySalary * 12) + yearlyBonus

    UPDATE departments
    SET departmentYearlySalary =
        departmentYearlySalary - ((OLD.monthlySalary * 12) + OLD.yearlyBonus)
    WHERE department_id = OLD.departmentId;

    UPDATE departments
    SET departmentYearlySalary =
        departmentYearlySalary + ((NEW.monthlySalary * 12) + NEW.yearlyBonus)
    WHERE department_id = NEW.departmentId;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `trgProfessorsAfterDelete` AFTER DELETE ON `professors` FOR EACH ROW BEGIN

    -- Purpose:
    -- This trigger runs automatically when a professor record is deleted.
    --
    -- It subtracts the deleted professor's yearly salary contribution
    -- from the departmentYearlySalary column of the related department.
    --
    -- This ensures that the department's total salary remains correct
    -- after professor removal.
    --
    -- Salary contribution formula:
    -- (monthlySalary * 12) + yearlyBonus

    UPDATE departments
    SET departmentYearlySalary =
        departmentYearlySalary - ((OLD.monthlySalary * 12) + OLD.yearlyBonus)
    WHERE department_id = OLD.departmentId;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `students`
--

DROP TABLE IF EXISTS `students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `students` (
  `student_id` int NOT NULL AUTO_INCREMENT,
  `student_no` varchar(20) NOT NULL,
  `first_name` varchar(60) NOT NULL,
  `last_name` varchar(60) NOT NULL,
  `sex` enum('M','F','X') NOT NULL,
  `birth_date` date NOT NULL,
  `email` varchar(120) NOT NULL,
  `year_level` tinyint NOT NULL,
  `status` enum('ACTIVE','LOA','GRADUATED','DROPPED') NOT NULL DEFAULT 'ACTIVE',
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`student_id`),
  UNIQUE KEY `student_no` (`student_no`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `students`
--

LOCK TABLES `students` WRITE;
/*!40000 ALTER TABLE `students` DISABLE KEYS */;
INSERT INTO `students` VALUES (1,'2025-0001','Liam','Santos','M','2005-01-15','liam.santos@uni.test',1,'ACTIVE','2026-02-26 23:11:58'),(2,'2025-0002','Mia','Cruz','F','2004-08-22','mia.cruz@uni.test',2,'ACTIVE','2026-02-26 23:11:58'),(3,'2025-0003','Noah','Reyes','M','2003-03-09','noah.reyes@uni.test',3,'ACTIVE','2026-02-26 23:11:58'),(4,'2025-0004','Ava','Garcia','F','2002-11-30','ava.garcia@uni.test',4,'ACTIVE','2026-02-26 23:11:58'),(5,'2025-0005','Elijah','Flores','M','2004-05-18','elijah.flores@uni.test',2,'ACTIVE','2026-02-26 23:11:58'),(6,'2025-0006','Sophia','Lim','F','2005-09-01','sophia.lim@uni.test',1,'ACTIVE','2026-02-26 23:11:58'),(7,'2025-0007','Lucas','Dizon','M','2003-12-12','lucas.dizon@uni.test',3,'LOA','2026-02-26 23:11:58'),(8,'2025-0008','Isla','Navarro','F','2002-06-25','isla.navarro@uni.test',4,'ACTIVE','2026-02-26 23:11:58'),(9,'2025-0009','Mateo','Tan','M','2004-02-14','mateo.tan@uni.test',2,'ACTIVE','2026-02-26 23:11:58'),(10,'2025-0010','Zoe','Velasco','F','2003-07-07','zoe.velasco@uni.test',3,'ACTIVE','2026-02-26 23:11:58');
/*!40000 ALTER TABLE `students` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `role` varchar(30) NOT NULL DEFAULT 'User',
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `username` (`username`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'admin','1','admin','admin','admin@admin.com',1,'2026-05-09 19:51:39','2026-05-09 21:40:18','User'),(2,'testupdate','test','test','test','test@test.com',1,'2026-05-09 19:53:55','2026-05-09 21:42:05','User'),(3,'mickole','mickole','mickole','dechavez','mickoledechavez@gmail.com',1,'2026-05-09 20:12:09','2026-05-09 20:13:14','Admin'),(4,'test2','test2','test2','test2','test2@gmail.com',1,'2026-05-09 21:04:12','2026-05-09 21:04:12','User'),(5,'test3','test3','test3','test3','test3@gmail.com',1,'2026-05-09 21:04:50','2026-05-09 21:04:50','User'),(6,'adduser','123','adduser','testuser','updatedemailuser@gmail.com',1,'2026-05-09 21:44:06','2026-05-09 21:44:50','User');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `v_course_enrollment_summary`
--

DROP TABLE IF EXISTS `v_course_enrollment_summary`;
/*!50001 DROP VIEW IF EXISTS `v_course_enrollment_summary`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_course_enrollment_summary` AS SELECT 
 1 AS `term`,
 1 AS `course_code`,
 1 AS `course_title`,
 1 AS `department_code`,
 1 AS `department_name`,
 1 AS `total_enrolled`,
 1 AS `total_dropped`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_department_gpa_report`
--

DROP TABLE IF EXISTS `v_department_gpa_report`;
/*!50001 DROP VIEW IF EXISTS `v_department_gpa_report`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_department_gpa_report` AS SELECT 
 1 AS `term`,
 1 AS `department_code`,
 1 AS `department_name`,
 1 AS `graded_count`,
 1 AS `avg_grade`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_student_schedule`
--

DROP TABLE IF EXISTS `v_student_schedule`;
/*!50001 DROP VIEW IF EXISTS `v_student_schedule`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_student_schedule` AS SELECT 
 1 AS `term`,
 1 AS `student_no`,
 1 AS `student_name`,
 1 AS `course_code`,
 1 AS `course_title`,
 1 AS `units`,
 1 AS `enrollment_status`*/;
SET character_set_client = @saved_cs_client;

--
-- Dumping events for database 'uni_db'
--

--
-- Dumping routines for database 'uni_db'
--
/*!50003 DROP FUNCTION IF EXISTS `fn_student_age` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_student_age`(p_student_id INT) RETURNS int
    READS SQL DATA
    DETERMINISTIC
BEGIN
  DECLARE v_birth DATE;

  SELECT birth_date INTO v_birth
  FROM students
  WHERE student_id = p_student_id;

  IF v_birth IS NULL THEN
    RETURN NULL;
  END IF;

  RETURN TIMESTAMPDIFF(YEAR, v_birth, CURDATE());
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_enroll_student` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_enroll_student`(
  IN p_student_id INT,
  IN p_course_id INT,
  IN p_term VARCHAR(10)
)
BEGIN
  DECLARE v_student_ok INT DEFAULT 0;
  DECLARE v_course_ok INT DEFAULT 0;
  DECLARE v_enrollment_id INT DEFAULT NULL;
  DECLARE v_enrollment_status VARCHAR(20) DEFAULT NULL;

  SELECT COUNT(*) INTO v_student_ok
  FROM students
  WHERE student_id = p_student_id
    AND status = 'ACTIVE';

  IF v_student_ok = 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Student not found or not ACTIVE.';
  END IF;

  SELECT COUNT(*) INTO v_course_ok
  FROM courses
  WHERE course_id = p_course_id;

  IF v_course_ok = 0 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Course not found.';
  END IF;

  SELECT enrollment_id, status
  INTO v_enrollment_id, v_enrollment_status
  FROM enrollments
  WHERE student_id = p_student_id
    AND course_id = p_course_id
    AND term = p_term
  LIMIT 1;

  IF v_enrollment_status = 'ENROLLED' THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Student is already enrolled in this course for the selected term.';
  END IF;

  IF v_enrollment_status = 'DROPPED' THEN
    UPDATE enrollments
    SET status = 'ENROLLED',
        enrolled_at = CURRENT_TIMESTAMP
    WHERE enrollment_id = v_enrollment_id;

    INSERT INTO grades(enrollment_id, grade, remarks)
    VALUES (v_enrollment_id, NULL, NULL)
    ON DUPLICATE KEY UPDATE
      grade = NULL,
      remarks = NULL;
  ELSE
  INSERT INTO enrollments(student_id, course_id, term, status)
  VALUES (p_student_id, p_course_id, p_term, 'ENROLLED');

  -- Create grade placeholder (NULL until grading)
  INSERT INTO grades(enrollment_id, grade, remarks)
  VALUES (LAST_INSERT_ID(), NULL, NULL);
  END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `v_course_enrollment_summary`
--

/*!50001 DROP VIEW IF EXISTS `v_course_enrollment_summary`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `v_course_enrollment_summary` AS select `e`.`term` AS `term`,`c`.`course_code` AS `course_code`,`c`.`course_title` AS `course_title`,`d`.`department_code` AS `department_code`,`d`.`department_name` AS `department_name`,count(0) AS `total_enrolled`,sum((`e`.`status` = 'DROPPED')) AS `total_dropped` from ((`enrollments` `e` join `courses` `c` on((`c`.`course_id` = `e`.`course_id`))) join `departments` `d` on((`d`.`department_id` = `c`.`department_id`))) group by `e`.`term`,`c`.`course_code`,`c`.`course_title`,`d`.`department_code`,`d`.`department_name` order by `e`.`term` desc,`c`.`course_code` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_department_gpa_report`
--

/*!50001 DROP VIEW IF EXISTS `v_department_gpa_report`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `v_department_gpa_report` AS select `e`.`term` AS `term`,`d`.`department_code` AS `department_code`,`d`.`department_name` AS `department_name`,count(`g`.`grade_id`) AS `graded_count`,round(avg(`g`.`grade`),2) AS `avg_grade` from (((`departments` `d` join `courses` `c` on((`c`.`department_id` = `d`.`department_id`))) join `enrollments` `e` on((`e`.`course_id` = `c`.`course_id`))) left join `grades` `g` on((`g`.`enrollment_id` = `e`.`enrollment_id`))) group by `e`.`term`,`d`.`department_code`,`d`.`department_name` order by `e`.`term` desc,`d`.`department_code` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_student_schedule`
--

/*!50001 DROP VIEW IF EXISTS `v_student_schedule`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `v_student_schedule` AS select `e`.`term` AS `term`,`s`.`student_no` AS `student_no`,concat(`s`.`last_name`,', ',`s`.`first_name`) AS `student_name`,`c`.`course_code` AS `course_code`,`c`.`course_title` AS `course_title`,`c`.`units` AS `units`,`e`.`status` AS `enrollment_status` from ((`enrollments` `e` join `students` `s` on((`s`.`student_id` = `e`.`student_id`))) join `courses` `c` on((`c`.`course_id` = `e`.`course_id`))) order by `e`.`term`,`s`.`student_no`,`c`.`course_code` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-05-16 17:26:09
