<?php


$output["DataCount"] = 0;
$output["EditorDataCount"] = 0;
foreach ($_POST as $entry) {
	$json = json_decode($entry);
	//var_dump($json);


	$SqlQueryData = "INSERT INTO `Data`
	(`ID`, `AppID`, `Scene`, `Version`, `Platform`, `Device`, `Identifier`, `CPUFPS`, `GPUFPS`) VALUES
	(NULL,
	'".$json->App."', '".$json->Scene."', '".$json->Version."', '".$json->Platform."', '".$json->Device."',

	'".$json->Identifier."',

	'".$json->CPUFPS."', '".$json->GPUFPS."')
	";

	$mysqli->query($SqlQueryData);
	$output["DataCount"]++;

	if ($json->isEditor) {
		$SqlQueryData = "INSERT INTO `EditorData`
		(`ID`, `Identifier`, `Vertices`, `Triangles`, `DrawCalls`, `SetPassCalls`, `Batches`, `TextureCount`, `TextureBytes`) VALUES
		(NULL,
		'".$json->Identifier."', 

		'".$json->Vertices."', '".$json->Triangles."',
		'".$json->DrawCalls."', '".$json->SetPassCalls."',
		'".$json->Batches."', '".$json->TextureCount."',
		'".$json->TextureBytes."')
		";
		$mysqli->query($SqlQueryData);
		$output["EditorDataCount"]++;
	}
}

?>