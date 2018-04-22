function check() {
    if (document.getElementById('password').value ===
        document.getElementById('reenter-password').value) {
        document.getElementById('submit').disabled = false;
    } else {
        document.getElementById('submit').disabled = true;
    }
}