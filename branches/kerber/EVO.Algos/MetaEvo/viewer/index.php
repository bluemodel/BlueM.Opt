<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv="refresh" content="3; URL=index.php">
<title>MetaEvo_Viewer</title>
<link href="css/style.css" rel="stylesheet" type="text/css" />
</head>

<body>
<?php
include("library/dbconnect.php");
include("library/dbwork.php");

echo"<p class=\"header\">MetaEvo - SchedulingViewer</p>";
	
$infos = dbgetselected("metaevo_infos", "true = true", "id", false);

echo"
<p class=\"subheader\">Infos</p>
<table width=\"500\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">
  <tr>
    <th scope=\"col\">Eigenschaft</th>
    <th scope=\"col\">Wert</th>
  </tr>";
  for ($tmp = 0; $tmp < $infos[info][eintrage]+1; $tmp++) {
	  echo"
	  <tr>
		<td scope=\"col\">".$infos[$tmp][bezeichnung]."</td>
		<td scope=\"col\">".$infos[$tmp][wert]."</td>
	  </tr>";
  }
  echo"
</table><br />";	
	
$network = dbgetselected("metaevo_network", "type = 'server'", "", false);

echo"
<p class=\"subheader\">Server</p>
<table width=\"500\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">
  <tr>
    <th scope=\"col\">Name</th>
    <th scope=\"col\">Status</th>
    <th scope=\"col\">Timestamp</th>
  </tr>
  <tr>
    <td scope=\"col\">".$network[0][ipName]."</td>
    <td scope=\"col\">".$network[0][status]."</td>
    <td scope=\"col\">".$network[0][timestamp]."</td>
  </tr>
</table><br />";

$network = dbgetselected("metaevo_network", "type = 'client' ORDER BY ipName", "", false);

//Individuen zählen und durchsuchen
$individuums = dbgetselected("metaevo_individuums", "true = true", "", false);
for ($tmp = 0; $tmp < $individuums[info][eintrage]; $tmp++) {
	$pictures[$individuums[$tmp][ipName]] .= "<img src=\"pictures/".$individuums[$tmp][status].".jpg\" width=\"20\" height=\"20\" />";
}

echo"
<p class=\"subheader\">Clients</p>
<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">
  <tr>
    <th width=\"150\" scope=\"col\">Name</th>
    <th width=\"80\" scope=\"col\">Status</th>
    <th width=\"100\" scope=\"col\">Timestamp</th>
	<th width=\"100\" scope=\"col\">Speed-AV</th>
	<th width=\"100\" scope=\"col\">Speed-Low</th>
	<th width=\"60\" scope=\"col\">Individuums</th>
	<th width=\"600\" scope=\"col\"></th>
  </tr>";
  
  for ($tmp = 0; $tmp < $network[info][eintrage]; $tmp++) {
	  echo"
	  <tr>
		<td scope=\"col\">".$network[$tmp][ipName]."</td>
		<td scope=\"col\">".$network[$tmp][status]."</td>
		<td scope=\"col\">".$network[$tmp][timestamp]."</td>
		<td scope=\"col\">".$network[$tmp][speed_av]."</td>
		<td scope=\"col\">".$network[$tmp][speed_low]."</td>
		<td scope=\"col\">".(substr_count($pictures[$network[$tmp][ipName]],"true")+(substr_count($pictures[$network[$tmp][ipName]],"false")))." / ".substr_count($pictures[$network[$tmp][ipName]],"<img")." / ".$individuums[info][eintrage]."</td>
		<td scope=\"col\">".$pictures[$network[$tmp][ipName]]."</td>
	  </tr>";
  }
  
  echo"
</table><br />";

?>
</body>
</html>
