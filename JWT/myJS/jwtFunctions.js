function getExpirationDate(token) {
    var tokenParts = token.split(".");
    var payloadDecoded = JSON.parse(atob(tokenParts[1]));

    return new Date(payloadDecoded.exp * 1000);
}