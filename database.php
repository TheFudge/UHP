<?php
/*
$dbname = "d030eafe";
$dbhost = "w00f9768.kasserver.com";
$dbpasswd = "cCNfAcHbpuR75xsV";


$mysqli = new mysqli($dbhost, $dbname, $dbpasswd, $dbname);
*/
$dbname = "UHP";
$dbuser = "root";
$dbhost = "127.0.0.1";
$dbpasswd = "";


$mysqli = new mysqli($dbhost, $dbuser, $dbpasswd, $dbname);

/*
 * This is the "official" OO way to do it,
 * BUT $connect_error was broken until PHP 5.2.9 and 5.3.0.
 */
if ($mysqli->connect_error) {
    die('Connect Error (' . $mysqli->connect_errno . ') '
            . $mysqli->connect_error);
}

$mysqli->set_charset("utf8mb4");

?>