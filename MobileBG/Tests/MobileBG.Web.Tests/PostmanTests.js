//The URL used for this test is: https://localhost:44319/api/Models/ForMake?makeId=CC63619E-C891-448D-6AD7-08DAA522024B. The tests are runned for the Audi brand.

pm.test("Status test", function () {
    pm.response.to.have.status(200);
});

pm.test("Audi_Model test", function () {
    pm.response.to.have.jsonBody();
});

pm.test("Body contains string", () => {
    pm.expect(pm.response.text()).to.include("A3");
});

pm.test("Body contains string", () => {
    pm.expect(pm.response.text()).to.include("A5");
});

pm.test("Body dostn't contain string", () => {
    pm.expect(pm.response.text()).not.to.include("1895");
});