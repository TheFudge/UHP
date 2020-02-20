<?php
header('Content-Type: application/json');
$output = array("Status" => "OK");
$page = $_GET["action"];

require_once "Config.php";
require_once "database.php";

switch ($page) {

	case 'new-values':
		require_once "save-in-db.php";
		break;
	case 'get':
		require_once "get.php";
		break;
	case 'info':
		require_once "info.php";
		break;

	default:
		break;
}

echo json_encode($output);
?>