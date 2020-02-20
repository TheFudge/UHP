<?php

$select_normal = $RowsDataTable;

$op = "";
$first = true;
foreach ($select_normal as $value) {
	if (!$first) {
		$op .= ",";
	}
	$first = false;
	$op .= " AVG(`$value`) as `$value-AVG`, MAX(`$value`) as `$value-MAX`, MIN(`$value`) as `$value-MIN` ";
}

$SelectTablesNormal = $op;

$select_editor = $RowsDataEditorTable;

$op = "";
$first = true;
foreach ($select_editor as $value) {
	if (!$first) {
		$op .= ",";
	}
	$first = false;
	$op .= " AVG(`$value`) as `$value-AVG`, MAX(`$value`) as `$value-MAX`, MIN(`$value`) as `$value-MIN` ";
}

$SelectTablesEditor = $op;

?>