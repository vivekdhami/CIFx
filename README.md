# CIFx

Web api with endpoints for uploading zipped test results and downloading the test result zip file or individual test result.

Uses azure blob storage for storing zipped test results file.

Zipped files are unzipped on the fly when individual test results are requested.

An api endpoint exists for monitoring web app availability, which will be used by azure traffic manager. The web app can be deployed to 
azure web apps in different locations and the traffic manager performs load balancing by monitoring this api endpoint.

API is documented using swagger.

#CIFx.Tests

Unit tests for the api controller.
