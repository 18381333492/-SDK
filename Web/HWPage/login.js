

//获取账户信息
function hw_getInfo() {
    try {
        var result = window.lives.getAccount(); //获取账户信息
        if (result != 3) {
            result = JSON.parse(result);
            return result.account; //返回登录的账号
        }
        else {
            return null;
        }
    } catch (e) {
        alert(e.message);
    }
}


function setAccount(response) {
    var result = JSON.parse(response);
    if (result.code == 200) {
        //登录成功
        var account = result.account;
        alert("登录成功");
    }
}