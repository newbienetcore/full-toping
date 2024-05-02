FROM node:20.10.0-alpine
RUN addgroup app && adduser -S -G app app
USER app
WORKDIR /app
COPY package*.json .
COPY .npmrc .
RUN npm install
COPY . .
ENV API_PATH=http://api.localhost
EXPOSE 4200
ENTRYPOINT [ "npm", "start" ]




# # Shell form
# # /bin/sh
# # ubuntu /bin/bash
# CMD npm start

# Exec form
# CMD [ "npm", "start" ]: nếu có 2-3 dòng này thì cái cuối cùng sẽ được thực thi








# COPY "hello.txt" .
# COPY ["package.json", "package-lock.jsom"] .
# COPY "package*.json" .