var app = angular.module("myApp", []);

app.controller("myCtrl", function ($scope, $http, $window, $interval) {
    $scope.isAuthenticated = false;
    $scope.elapsedSeconds = 0;
    $scope.timeout = 15;

    // LOGIN
    $scope.login = function () {
        $scope.requestNewToken();
        $scope.intervalPromise = $interval($scope.pollSessionState, 1000);
    }

    // LOGOUT
    $scope.logout = function () {
        $window.sessionStorage.removeItem("jwt"); // eliminamos el token al hacer logout
        $scope.isAuthenticated = false;
        $window.location.href = "index.html";
    }

    $scope.requestNewToken = function () {
        $http({
            url: "api/login/authenticate",
            method: "POST",
            data: angular.toJson($scope.user),
            headers: { "Content-Type": "application/json; charset=utf-8" }
        })
        .then(
            response => {
                $window.sessionStorage.setItem("jwt", response.data); // guardamos el token en el session storage
                $scope.expirationDate = getExpirationDate($window.sessionStorage.getItem("jwt"));
                $scope.expirationTime = $scope.expirationDate.toLocaleTimeString();
                $scope.isAuthenticated = true;
            },
            response => {
                alert("Error: " + response.status + ". Message: " + response.statusText);
                $window.sessionStorage.removeItem("jwt"); // eliminamos cualquier token que hubiera guardado si hay error
                $scope.isAuthenticated = false;
            });
    }

    $scope.resetCount = function () {
        $scope.elapsedSeconds = 0;
    }

    $scope.pollSessionState = function () {
        if (new Date().getTime() >= $scope.expirationDate.getTime()) {
            $scope.requestNewToken();
        }

        if ($scope.elapsedSeconds >= $scope.timeout) {
            if (confirm("¿Desea continuar navegando?")) {
                $scope.elapsedSeconds = 0;
            }
            else {
                $interval.cancel($scope.intervalPromise);
                $scope.logout();
            }
        }

        $scope.remainingSeconds = $scope.timeout - $scope.elapsedSeconds;
        $scope.elapsedSeconds++;
    }

    // GET
    $scope.getUsers = function () {
        $http({
            url: "api/user2",
            method: "GET",
            headers: { "Authorization": "Bearer " + $window.sessionStorage.getItem("jwt") }
        })
            .then(function success(response) {
                $scope.records = response.data;
            },
                function error(response) {
                    alert("Error: " + response.status + ". Message: " + response.statusText);
                    $scope.logout();
                })
    }

    // POST
    $scope.addUser = function () {
        $http({
            url: "api/user2",
            method: "POST",
            data: angular.toJson($scope.usr),
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "Authorization": "Bearer " + $window.sessionStorage.getItem("jwt")
            }
        })
            .then(function success(response) {
                $scope.getUsers();
            },
                function error(response) {
                    alert("Error: " + response.status + ". Message: " + response.statusText);
                    $scope.logout();
                })
    }

    // PUT
    $scope.updateUserPass = function () {
        $http({
            url: "api/user2",
            method: "PUT",
            data: angular.toJson($scope.usr),
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "Authorization": "Bearer " + $window.sessionStorage.getItem("jwt")
            }
        })
            .then(function success(response) {
                $scope.getUsers();
            },
                function error(response) {
                    alert("Error: " + response.status + ". Message: " + response.statusText);
                    $scope.logout();
                })
    }

    // DELETE
    $scope.deleteUser = function () {
        $http({
            url: "api/user2",
            method: "DELETE",
            data: angular.toJson($scope.usr),
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "Authorization": "Bearer " + $window.sessionStorage.getItem("jwt")
            }
        })
            .then(function success(response) {
                $scope.getUsers();
            },
                function error(response) {
                    alert("Error: " + response.status + ". Message: " + response.statusText);
                    $scope.logout();
                })
    }
});