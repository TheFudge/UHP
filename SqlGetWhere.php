<?php

$WhereClause = " WHERE ";

$firstA = true;
foreach ($_REQUEST as $key => $value) {
	if ($key == "action") {
		continue;
	}
	if (trim($value) == "") {
		continue;
	}

	if (!$firstA) {
		$WhereClause .= " AND ";
	} $firstA = false;

	$entries = explode(";", $value);

	$WhereClause .= " (";
	$firstB = true;
	foreach ($entries as $innerKey => $innerValue) {
		if (!$firstB) {
			$WhereClause .= " OR ";
		} $firstB = false;
		$WhereClause .= " `$key`= '$innerValue' ";
	}
	$WhereClause .= ") ";
}
if (trim($WhereClause) == "WHERE") {
	$WhereClause = "";
}

?>