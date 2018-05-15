function check() {
    var textBox = document.getElementById("password");
    var textLength = textBox.value.length;

    if (document.getElementById('password').value ===
        document.getElementById('reenter-password').value) {
        document.getElementById('message').innerHTML = "";
        if (textLength > 5) {
            document.getElementById('submit').disabled = false;
        }
        else {
             document.getElementById('submit').disabled = true;
        }
 
    } else {
        document.getElementById('message').style.color = 'red';
        document.getElementById('message').innerHTML = "Passwords do not match";
        document.getElementById('submit').disabled = true;
    }
}

function checkLength() {
    check();
    var textBox = document.getElementById("password");
    var textLength = textBox.value.length;

    if (textBox.value == '' || textLength < 6) {
        document.getElementById('pw_message').style.color = 'red';
        document.getElementById('pw_message').innerHTML = "Password must be atleast 6 characters";
    }
    else {
        document.getElementById('pw_message').innerHTML = '';
    }
}

