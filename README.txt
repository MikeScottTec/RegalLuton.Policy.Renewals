Solution implemented as a WinForms application.

The location of the source customer.csv file and the folder for the output letters is defined in app.config

Keys:
CustomerInputSourceFile = C:\RoyalLuton\Source\Customer.csv
CustomerTargetFolder    = C:\RoyalLuton\Target

Please create these folders prior to running the application.   The application will handle the source customer.csv not existing, however, does not handle the target folder not existing.  This could be checked using Directory.Exists from the System.IO namespace.

Notes re: Unit Testing
I have not unit tested the FileHandler class, because this would require a mocking framework such as Moq and as per the specification I have not included this.

When using the mocking framework I would inject a mocked instance of the FileHandler class into the CusmtomerProcessor (which has an interface in the constructor signature in readiness).  Each individual mocked instance would be Setup to return both valid and invalid values for the read and exists methods.

The FileHandler Write methods would be Verified to ensure that the method is called the expected number of times.

The CustomerProcessorTests Unit Tests, do not check the contents of the parsedCustomers list (with the exception of the ParseCustomers_ShouldParseAllSuccessfully() test) for brevity reasons, it could be argued that the other tests do not need to do this, if this tests passes.