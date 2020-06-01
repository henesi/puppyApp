FROM python:3.6

# install Microsoft dependencies
ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps


# Install .NET Core
RUN dotnet_version=3.1.4 \
    && wget -O dotnet.tar.gz https://download.visualstudio.microsoft.com/download/pr/a1ddc998-933c-47af-b8c7-dc2503e44e91/42d8cd08b2055df52c9457c993911f2e/aspnetcore-runtime-3.1.4-linux-x64.tar.gz \
    && dotnet_sha512='a761fd3652a0bc838c33b2846724d21e82410a5744bd37cbfab96c60327c89ee89c177e480a519b0e0d62ee58ace37e2c2a4b12b517e5eb0af601ad9804e028f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz

WORKDIR /app

# install imageai dependencies
RUN pip3 install --upgrade pip setuptools virtualenv
ADD https://github.com/OlafenwaMoses/ImageAI/releases/download/1.0/yolo.h5 yolo.h5
COPY ./Dependencies/requirements.pip requirements.pip
RUN pip3 install -r requirements.pip
#COPY ./Dependencies/yolo.h5 yolo.h5

# copy application files
COPY ./Dependencies/object_detection.py .
COPY ./bin/Publish .
ENTRYPOINT ["dotnet", "ComputerVisionService.dll"]
