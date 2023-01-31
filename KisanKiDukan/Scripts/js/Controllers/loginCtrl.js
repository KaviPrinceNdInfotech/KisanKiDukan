/* <reference path="../Library/angular.js" />*/
var app = angular.module('myApp', []);

app.controller('loginCtrl', function ($scope, loginFactory) {
    $scope.login = function (user) {
        $scope.color = "grey";
        $scope.message = "matching your credentials......";
        loginFactory.login(user).then(function (response) {
            if (response.data == "ok")
                location.href = "/Admin/Dashboard";
            else {
                $scope.color = "red";
                $scope.message = response.data;
            }
        })
    }
});

app.factory('loginFactory', function ($http) {

    var fac = {};
    fac.login = function (user) {
        return $http({
            url: '/Admin/CheckUserAuthentication',
            method: 'post',
            data: user,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8'
        });
    }
    return fac;
});


