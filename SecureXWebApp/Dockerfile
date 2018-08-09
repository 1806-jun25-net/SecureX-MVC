# each command in the dockerfile makes a new layer
# which points to the parent layer and contains any
# filesystem or metadata changes.
# the final layer is the one we tag with a name
# like "restreviews:0.1".
# the layers that don't really change anything like
# "WORKDIR" are optimized away, really only
# COPY and RUN make full new layers.

# base image for this image.
FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY SecureXTest/*.csproj ./SecureXTest/
COPY SecureXWebApp/*.csproj ./SecureXWebApp/
RUN dotnet restore

# WORKDIR is "cd" in Dockerfile. later commands are run
# from this directory. creates the dir if it doesn't exist.

# first argument: where from my filesystem
# second argument: where inside Docker image
# copies current directory contents (RestaurantReviews/*)
# into current directory in Docker (/app)


# copy everything else and build app
COPY . ./
WORKDIR /app/SecureXWebApp
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/SecureXWebApp/out ./
ENTRYPOINT ["dotnet", "SecureXWebApp.dll"]
