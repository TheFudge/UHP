<?php
require_once "SqlGetWhere.php";
require_once "SqlStatementSelect.php";

$SqlQueryData = "SELECT Identifier, COUNT(`ID`) as `Count`, $SelectTablesNormal FROM `Data` $WhereClause GROUP BY Identifier";

$SqlQueryEditorData = "SELECT Identifier, COUNT(`ID`) as `Count`, $SelectTablesEditor FROM `EditorData` GROUP BY Identifier";

//$output["DataQuery"] = $SqlQueryData;
//$output["EditorDataQuery"] = $SqlQueryEditorData;

$ResultData = $mysqli->query($SqlQueryData);
$ResultEditorData = $mysqli->query($SqlQueryEditorData);
$points = array();


function GetEntryFromRow($row, $value)
{
	return array(
		"Min" => doubleval($row["$value-MIN"]),
		"Avg" => doubleval($row["$value-AVG"]),
		"Max" => doubleval($row["$value-MAX"]),
		"Count" => intval($row["Count"])
	);
}


while ($row = $ResultData->fetch_assoc()) {
	$Identifier = $row["Identifier"];
	$entry = array("Identifier" => $row["Identifier"]);

	foreach ($RowsDataTable as $element) {
		$entry[$element] = GetEntryFromRow($row, $element);
	}

	$points[$Identifier] = $entry;
}


while ($row = $ResultEditorData->fetch_assoc()) {
	
	$Identifier = $row["Identifier"];

	if (isset($points[$Identifier])) {
		$entry = $points[$Identifier];
	} else {
		continue;
	}

	foreach ($RowsDataEditorTable as $element) {
		$entry[$element] = GetEntryFromRow($row, $element);
	}

	$points[$Identifier] = $entry;
}

$outputpoints = array();
foreach ($points as $key => $value) {
	$outputpoints[] = $value;

}
$output = array("Points" => $outputpoints);

?>