FROM node:22.12-alpine
WORKDIR /app
COPY . .
RUN npm install
RUN npm install -g dotenv-cli
EXPOSE 5272
CMD ["npm", "run", "docker"]