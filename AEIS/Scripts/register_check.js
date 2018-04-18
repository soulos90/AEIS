// JavaScript source code
<script src="//code.jquery.com/jquery-2.1.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var check = function () {
		if (document.getElementById('password').value == document.getElementById('reenter-password').value {
            document.getElementById('message').style.color = 'green';
        document.getElementById('message').innerHTML = 'matching';
		} else {
            document.getElementById('message').style.color = 'red';
        document.getElementById('message').innerHTML = 'not matching';
		}
	}

</script>