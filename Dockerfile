FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development

# Instalar herramientas básicas del sistema y depurador
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    procps \
    curl \
    unzip \
    && rm -rf /var/lib/apt/lists/*

# Instalar el depurador de VS Code
RUN curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

WORKDIR /src

# Copiar la solución y el proyecto
COPY *.sln .
COPY PROY/*.csproj ./PROY/
RUN dotnet restore

# Copiar el resto de los archivos
COPY . .

WORKDIR /src/PROY  

# Exponer puerto
EXPOSE 8088

# Comando para desarrollo con watch
CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:8088", "--non-interactive"]