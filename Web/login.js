


//获取账户信息
function hw_getInfo() {
    var  account = localStorage.getItem("accountInfo");
    if (account) {
        return account;
    }
    else {
        var result = window.lives.getAccount();//获取账户信息
        if (result != "") {
            result = JSON.parse(result);
            localStorage.setItem("accountInfo", result.account);
            return result.account;//返回登录的账号
        }
        else {
            return "";
        }
    }
}

location.href="hwpayment.html";

//登录成功 华为异步调用
function setAccount(response) {
    var result = JSON.parse(response);
    if (result.code==200) {
        //登录成功
        var account = result.account;
        localStorage.setItem("accountInfo", account);
    }
    else {
        //登录失败
        alert(result.code+"  "+result.message);
    }
}

try{
    var account = localStorage.getItem("accountInfo");
    alert(account)
    if (account) {
        alert(-1);
        return account;
    }
    else {
        alert(0);
        alert(window.hw_lives);
        alert(window.lives);
        var result = window.hw_lives.getAccount(); //获取账户信息
        if (result != "") {
            result = JSON.parse(result);
            localStorage.setItem("accountInfo", result.account);
            return result.account; //返回登录的账号
        }
        else {
            return "";
        }
    }
}
catch(e){
    alert(e.Message)
}