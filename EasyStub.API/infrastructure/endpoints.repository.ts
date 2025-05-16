import { Mutex } from "async-mutex";
import { Endpoint, EndpointId, UnsavedEndpoint } from "../Endpoint.model";

// to be thread-safe
const mutex = new Mutex();

export class EndpointsRepository {
  endpoints: Record<EndpointId, Endpoint | undefined> = {};

  public async create(endpoint: UnsavedEndpoint): Promise<Endpoint> {
    return await mutex.runExclusive(() => {
      const id = this.getNextId();

      const newEndpoint = { ...endpoint, id };

      this.endpoints[id] = newEndpoint;

      return newEndpoint;
    });
  }

  private getNextId() {
    const existingIds = Object.values(this.endpoints).map((x) => x?.id ?? 0);

    const biggestId = existingIds.reduce(
      (previous, current) => (current > previous ? current : previous),
      0
    );

    return biggestId + 1;
  }

  public async remove(id: EndpointId): Promise<Endpoint | undefined> {
    return await mutex.runExclusive(() => {
      const found = this.endpoints[id];

      if (found != null) {
        this.endpoints[id] = undefined;
      }

      return found;
    });
  }

  public getAll(): Endpoint[] {
    return Object.values(this.endpoints).flatMap((x) => (x ? [x] : []));
  }
}
