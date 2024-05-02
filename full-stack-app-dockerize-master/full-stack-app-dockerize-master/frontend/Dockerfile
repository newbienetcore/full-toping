FROM node:12.11.1-alpine
RUN addgroup app && adduser -S -G app app
USER app
WORKDIR /app
COPY package*.json .
RUN npm install
COPY . .
ENV API_PATH=http://api.localhost
EXPOSE 4200
ENTRYPOINT [ "npm", "start" ]


