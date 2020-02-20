<?php

$fetch = array('Device' => "Devices", "AppID" => "Apps", "Scene" => "Scenes", "Version" => "Versions", "Platform" => "Platforms");

foreach ($fetch as $key => $value) {
	$sql = "SELECT `$key` FROM Data GROUP BY `$key`";

	$result = $mysqli->query($sql);
	$output[$value] = array();

	while ($row = $result->fetch_assoc()) {
		$output[$value][] = $row[$key];
	}
}

$sql = "SELECT COUNT(ID) as Entries FROM Data";
$result = $mysqli->query($sql);
$row = $result->fetch_assoc();
$output["DataEntries"] = intval($row["Entries"]);
$sql = "SELECT COUNT(ID) as Entries FROM EditorData";
$result = $mysqli->query($sql);
$row = $result->fetch_assoc();
$output["EditorEntries"] = intval($row["Entries"]);
?>