<?php
//@mysql_query("SET NAMES 'utf8'");   UTF8 damit in sql und html gleich?

function dbupdate($table,$vergleich,$datenarray,$debug) {
	$edit = "UPDATE ".$table." SET ";
		foreach ($datenarray as $key => $eintrag) {
			$edit .= $key." = '".$eintrag."', "; 
		}
	$edit = rtrim($edit,", ");	
	$edit .= " WHERE ".$vergleich;
	if($debug) {//echo "<br /><br />Das bekommt dbupdate: "; print_r($datenarray); echo"<br />";
		echo "<br /><br />Das bekommt dbupdate: ".$edit;
	}
	else if(mysql_query($edit)or die("<b>Fehler bei der Datenbankanfrage: </b>".mysql_error())){return true;}
	else return false;
}

function dbinsert($table,$datenarray,$debug) {
	$edit = "INSERT INTO ".$table." ("; 
	$values = ") VALUES (";
	foreach ($datenarray as $key => $eintrag) {
			$edit .= $key.", ";
			$values .= "'".$eintrag."', "; 
		}
	$edit = rtrim($edit,", ");	
	$values = rtrim($values,", ");	
	$edit .= $values.")"; 
	if($debug) {//echo "<br /><br />Das bekommt dbinsert: "; print_r($datenarray); echo"<br />";
		echo "<br /><br />Das bekommt dbinsert: ".$edit;
	}
	else if(mysql_query($edit)or die("<b>Fehler bei der Datenbankanfrage: </b>".mysql_error())){return true;}
	else return false;
}

function dbdelete($table,$vergleich,$debug) {
	$delete= "DELETE FROM ".$table." WHERE ".$vergleich."  LIMIT 1";
	if($debug) {//echo "<br /><br />Das bekommt dbdelete: "; print_r($datenarray); echo"<br />";
		echo "<br /><br />Das bekommt dbdelete: ".$delete;
	}
	else if(mysql_query($delete)or die("<b>Fehler bei der Datenbankanfrage: </b>".mysql_error())){return true;}
	else return false;
}

function dbgetselected($table, $vergleich, $dbkey, $debug) {
	$ask= "SELECT * FROM ".$table." WHERE ".$vergleich;
	$content[info][eintrage] = 0;
	
	$do = mysql_query($ask);
	$index = 0;
	while($row = mysql_fetch_object($do)){
		foreach ($row as $key => $value) {
			if ($dbkey == "") $content[$index][$key] = $value;	
			else $content[$row->$dbkey][$key] = $value;	
		}
		$index++;
		$content[info][eintrage]++;
	}
	if ($debug) {
		echo"<br /><br />Das bekommt dbgetselected: ".$ask;
		echo"<br /><br />Das liefert dbgetselected: ";
		print_r($content);
		return false;
	}
	else {
		return $content;
	}
}
?>