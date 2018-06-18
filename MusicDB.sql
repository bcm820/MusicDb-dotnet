SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema MusicDB
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `MusicDB` DEFAULT CHARACTER SET utf8 ;
USE `MusicDB` ;

-- -----------------------------------------------------
-- Table `MusicDB`.`Artists`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `MusicDB`.`Artists` (
  `Id` INT(11) NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  `Url` VARCHAR(255) NOT NULL,
  `Image` VARCHAR(255) NOT NULL,
  `Genius` VARCHAR(255) NOT NULL,
  `Instagram` VARCHAR(255) NULL DEFAULT NULL,
  `Facebook` VARCHAR(255) NULL DEFAULT NULL,
  `Twitter` VARCHAR(255) NULL DEFAULT NULL,
  `CreatedAt` DATETIME NOT NULL,
  `UpdatedAt` DATETIME NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `MusicDB`.`Users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `MusicDB`.`Users` (
  `Id` INT(11) NOT NULL,
  `Username` VARCHAR(255) NOT NULL,
  `Password` VARCHAR(255) NOT NULL,
  `CreatedAt` DATETIME NOT NULL,
  `UpdatedAt` DATETIME NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `MusicDB`.`ArtistLikes`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `MusicDB`.`ArtistLikes` (
  `UserId` INT(11) NOT NULL,
  `ArtistId` INT(11) NOT NULL,
  `CreatedAt` DATETIME NOT NULL,
  `UpdatedAt` DATETIME NOT NULL,
  PRIMARY KEY (`UserId`, `ArtistId`),
  INDEX `fk_User_has_Artist_Artist1_idx` (`ArtistId` ASC),
  INDEX `fk_User_has_Artist_User1_idx` (`UserId` ASC),
  CONSTRAINT `fk_User_has_Artist_Artist1`
    FOREIGN KEY (`ArtistId`)
    REFERENCES `MusicDB`.`Artists` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_User_has_Artist_User1`
    FOREIGN KEY (`UserId`)
    REFERENCES `MusicDB`.`Users` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `MusicDB`.`Songs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `MusicDB`.`Songs` (
  `Id` INT(11) NOT NULL,
  `Title` VARCHAR(255) NOT NULL,
  `Url` VARCHAR(255) NOT NULL,
  `ArtistName` VARCHAR(255) NOT NULL,
  `ArtistId` INT(11) NOT NULL,
  `CreatedAt` DATETIME NOT NULL,
  `UpdatedAt` DATETIME NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_Song_Artist_idx` (`ArtistId` ASC),
  CONSTRAINT `fk_Song_Artist`
    FOREIGN KEY (`ArtistId`)
    REFERENCES `MusicDB`.`Artists` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `MusicDB`.`SongLikes`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `MusicDB`.`SongLikes` (
  `UserId` INT(11) NOT NULL,
  `SongId` INT(11) NOT NULL,
  `CreatedAt` DATETIME NOT NULL,
  `UpdatedAt` DATETIME NOT NULL,
  PRIMARY KEY (`UserId`, `SongId`),
  INDEX `fk_User_has_Song_Song1_idx` (`SongId` ASC),
  INDEX `fk_User_has_Song_User1_idx` (`UserId` ASC),
  CONSTRAINT `fk_User_has_Song_Song1`
    FOREIGN KEY (`SongId`)
    REFERENCES `MusicDB`.`Songs` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_User_has_Song_User1`
    FOREIGN KEY (`UserId`)
    REFERENCES `MusicDB`.`Users` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;